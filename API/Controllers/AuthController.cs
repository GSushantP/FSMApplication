using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JwtTokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginCreds creds)
    {
        if(creds.Username == "admin" && creds.Password == "password")
        {
            var token = tokenService.GenerateToken(creds.Username, "Admin");
            return Ok(new {Token = token});
        }
        return Unauthorized("Invalid Credentials");
    }
}
