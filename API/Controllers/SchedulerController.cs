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
        var assignedRequest = await schedulerService.AssignTechnicianAsync(requestId);
        if(assignedRequest == null)
        {
            return NotFound("Service Request not fount or already completed");
        }
        return Ok(assignedRequest);
    }
}
