using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Http.Features;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Services;

public class SchedulerService(ApplicationDbContext context)
{
    public async Task<ServiceRequest> ScheduleTechnicianAsync(int requestId)
    {
        var request = await context.ServiceRequests
        .FirstOrDefaultAsync(r => r.Id == requestId);
        if(request == null)
        {
            throw new Exception("Service request not found");
        }
        if(request.Status == "Assigned")
        {
            throw new Exception("Service request is already assigned or not available for assignment");
        }

        var technicians = await context.Technicians
        .Where(t => t.IsAvailable)
        .ToListAsync();

        if(!technicians.Any())
        {   
            throw new Exception("No Available technicians");
        }

        //Initialize OR-Tools Solver
        var solver = Solver.CreateSolver("SCIP");
        if(solver == null)
        {
            throw new Exception("Failed to create Solver");
        }

       // Create a matrix of decision variables
       //decision[i, j] is 1 if technician i is assigned to request j, else 0
        var decisionVars = new Variable[technicians.Count];
        for(int i=0; i<technicians.Count; i++)
        {
            decisionVars[i] = solver.MakeIntVar(0, 1, $"x_{i}");
        }

        //Minimize total distance between technicians and requests
        var objective = solver.Objective();
        for (int i = 0; i < technicians.Count; i++)
        {
            double distance = CalculateDistance(technicians[i].Latitude, technicians[i].Longitude, request.Latitude, request.Longitude);
                objective.SetCoefficient(decisionVars[i], distance);
        }
        objective.SetMinimization();

        //Constraints:
        //1. Each request should be assign to exactly one Technician
        var requestCinstraint = solver.MakeConstraint(1, 1, $"request_assignment");
            for (int i = 0; i < technicians.Count; i++)
            {
                requestCinstraint.SetCoefficient(decisionVars[i], 1);
            }
        //2. Each technician can be assigned to at most one request
        for (int i = 0; i < technicians.Count; i++)
        {
            var technicianConstraint = solver.MakeConstraint(0, 1, $"technician_{i}_assignment");
            technicianConstraint.SetCoefficient(decisionVars[i], 1);
        }

        //solve the problem
        var resultStatus = solver.Solve();
        if (resultStatus != Solver.ResultStatus.OPTIMAL)
        {
            throw new Exception("The Solver did not find an optimal solution");
        }

        //Find the assigned technician
        Technician assignedTechnician = null;
        for (int i = 0; i < technicians.Count; i++)
        {
            if(decisionVars[i].SolutionValue() > 0.5)
            {
                assignedTechnician = technicians[i];
                break;
            }
        }
        if(assignedTechnician == null)
        {
            throw new Exception("No suitable technician found");
        }

        //Update the service request and technician availability
        request.TechnicianId = assignedTechnician.Id;
        request.Status = "Assigned";
        assignedTechnician.IsAvailable = false;

        await context.SaveChangesAsync();
        return request;
    }

    //formula for calculation distance between two lat/long points
    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double EarthRadiusKm = 6371.0;
        var dLat = DegreesToRadians(lat2 - lat1);
        var dLon = DegreesToRadians(lon2 - lon1);

        var a = Math.Sin(dLat/2) * Math.Sin(dLat/2) +
        Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
        Math.Sin(dLon/2) * Math.Sin(dLon/2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
        return EarthRadiusKm * c;
    }

    private double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    public async Task<ServiceRequest> UnassignTechnicianAsync(int requestId)
    {
        var request = await context.ServiceRequests.Include(sr => sr.Technician)
        .FirstOrDefaultAsync(r => r.Id == requestId);

        if(request == null)
        {
            return null;
        }

        if(request.TechnicianId == null)
        {
            throw new Exception("No technician is currently assigned to the request.");
        }

        var technician = await context.Technicians.FindAsync(request.TechnicianId);
        if(technician != null)
        {
            technician.IsAvailable = true;
        }

        request.TechnicianId = null;
        request.Status = "Pending";

        await context.SaveChangesAsync();

        return request;
    }
}
