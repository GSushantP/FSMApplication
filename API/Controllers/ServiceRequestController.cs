using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceRequestController(ApplicationDbContext context) : ControllerBase
{
    // GET: api/servicerequest
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
    {
        return await context.ServiceRequests.ToListAsync();
    }

    //GET: api/servicerequest/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceRequest>> GetServiceRequest(int id)
    {
        var serviceRequest = await context.ServiceRequests.FindAsync(id);
        if(serviceRequest == null)
        {
            return NotFound();
        }
        return serviceRequest;
    }

    //POST: api/servicerequest
    [HttpPost]
    public async Task<ActionResult<ServiceRequest>> CreateServiceRequest([FromBody] ServiceRequest serviceRequest)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        context.ServiceRequests.Add(serviceRequest);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetServiceRequest), new {id = serviceRequest.Id}, serviceRequest);
    }

    //PUT: api/servicerequest/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceRequest>> UpdateServiceRequest(int id, ServiceRequest serviceRequest)
    {
        if(id != serviceRequest.Id)
        {
            return BadRequest();
        }
        context.Entry(serviceRequest).State = EntityState.Modified;
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if(!context.ServiceRequests.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }

    //DELETE: api/servicerequest/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServicerequest(int id)
    {
        var serviceRequest = await context.ServiceRequests.FindAsync(id);
        if(serviceRequest == null)
        {
            return NotFound();
        }

        context.ServiceRequests.Remove(serviceRequest);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
