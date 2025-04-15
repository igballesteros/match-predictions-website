using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi_pred.Data;
using webapi_pred.Models;
using Microsoft.AspNetCore.Authorization;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public class LoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        // Add null check for JWT config
        var jwtKey = _config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing in configuration");
        var jwtIssuer = _config["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is missing");
        var jwtAudience = _config["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience is missing");

        var user = _context.Users.FirstOrDefault(u => u.Username == loginDto.Username);

        if (user == null)
            return Unauthorized("User not found");

        if (user.Password != loginDto.Password)
            return Unauthorized("Wrong password");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            user.Username,
            user.Role,
            userId = user.UserId
        });
    }
}