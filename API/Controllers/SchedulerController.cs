using API.Data;
using API.DTOs;
using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]

public class SchedulerController(SchedulerService schedulerService, ApplicationDbContext context) : ControllerBase
{
    [HttpGet("assigned-tasks")] //api/scheduler/assigned-tasks
    public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetAssignedTasks()
    {
        var assignedTasks = await context.ServiceRequests
            .Where(sr => sr.TechnicianId != null)
            .Select(sr => new AssignedTaskDto
            {
                TaskId = sr.Id,
                TechnicianName = sr.Technician.Name
            })
            .ToListAsync();

        return Ok(assignedTasks);
    }
    [HttpPost ("assign-technician/{requestId}")]
    public async Task<IActionResult> AssignTechnician(int requestId)
    {
        try
        {
            var scheduleRequest = await schedulerService.ScheduleTechnicianAsync(requestId);
            if (scheduleRequest == null)
            {
                return NotFound($"Service request with ID {requestId} not found");
            }
            return Ok(scheduleRequest);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    //unassign technician API Endpoint
    [HttpPost("unassign-technician/{requestId}")]
    public async Task<IActionResult> UnassignTechnician(int requestId)
    {
        try
        {
            await schedulerService.UnassignTechnicianAsync(requestId);
            return Ok("Unassigned");
        }
        catch (Exception ex)
        {
           return StatusCode(500, ex.Message);
        }
    }
}
