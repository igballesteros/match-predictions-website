using SharedDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_pred.Data;
using webapi_pred.Models;
using Microsoft.AspNetCore.Authorization;

namespace webapi_pred.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
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
                Points = u.Points
            });

            return Ok(userDtos);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // return a DTO
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.UserId },
                new UserDto
                {
                    UserId = newUser.UserId,
                    Username = newUser.Username,
                    Points = newUser.Points
                });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Points = user.Points
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Update only allowed fields from DTO
            user.Username = userDto.Username;
            user.Points = userDto.Points;

            await _context.SaveChangesAsync();
            return NoContent();
        }

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