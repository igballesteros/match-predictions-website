using SharedDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_pred.Data;
using webapi_pred.Models;
using Microsoft.AspNetCore.Authorization;

namespace webapi_pred.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            // Map User to UserDto
            var userDtos = users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Points = u.Points,
                Role = u.Role
            });

            return Ok(userDtos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // return a DTO
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.UserId },
                new UserDto
                {
                    UserId = newUser.UserId,
                    Username = newUser.Username,
                    Points = newUser.Points,
                    Role = newUser.Role
                });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Points = user.Points,
                Role = user.Role
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Update only allowed fields from DTO
            user.Username = userDto.Username;
            user.Points = userDto.Points;
            user.Role = userDto.Role;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}