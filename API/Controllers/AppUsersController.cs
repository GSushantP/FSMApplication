using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppUsersController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.AppUsers.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await context.AppUsers.FindAsync(id);
        if(user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<AppUser>> PostUser(AppUser user)
    {
        context.AppUsers.Add(user);
        await context.SaveChangesAsync();
        return CreatedAtAction("GetUser", new {id = user.Id}, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, AppUser user)
    {
        if(id != user.Id)
        {
            return BadRequest();
        }
        context.Entry(user).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var user = await context.AppUsers.FindAsync(id);
        if(user == null)
        {
            return NotFound();
        }

        context.AppUsers.Remove(user);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
