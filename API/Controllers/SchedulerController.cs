using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]

public class SchedulerController(SchedulerService schedulerService) : ControllerBase
{
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
