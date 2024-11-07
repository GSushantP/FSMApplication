using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TechniciansController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Technician>>> GetTechnicians()
    {
        var technicians = await context.Technicians
        .ToListAsync();
        return Ok(technicians);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Technician>> GetTechnician(int id)
    {
        var technician = await context.Technicians.FindAsync(id);
        if(technician == null)
        {
            return NotFound();
        }
        return Ok(technician);
    }
    [HttpPost]
    public async Task<ActionResult<Technician>> PostTechnician([FromBody] Technician technician)
    {
        if(technician == null)
        {
            return BadRequest("data is required");
        }
        context.Technicians.Add(technician);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new {id = technician.Id}, technician);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<Technician>> PutTechnician(int id, Technician technician)
    {
        if(id != technician.Id)
        {
            return BadRequest();
        }
        context.Entry(technician).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTechnician(int id)
    {
        var technician = await context.Technicians.FindAsync(id);
        if(technician == null)
        {
            return NotFound();
        }
        context.Technicians.Remove(technician);
        await context.SaveChangesAsync();

        return Ok("Deleted");
    }
}
