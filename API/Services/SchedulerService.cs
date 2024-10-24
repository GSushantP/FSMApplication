using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class SchedulerService(ApplicationDbContext context)
{
    public async Task<ServiceRequest> AssignTechnicianAsync(int requestId)
    {
        var request = await context.ServiceRequests.FindAsync(requestId);
        if(request == null || request.Status =="Completed")
        {
            return null;
        }
        //Finding available technician based on Criteria (ie. nearest, available, skilled)
        var technician = await context.AppUsers
            .Where(t => t.Role == "Technician").FirstOrDefaultAsync();

        if(technician == null)
        {
            throw new Exception("No Available technicians.");
        }
        // Assign the technician
        request.TechnicianId = technician.Id;
        request.Status = "Assigned";

        await context.SaveChangesAsync();
        return request;
    }
}
