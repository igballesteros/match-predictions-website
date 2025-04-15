using SharedDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_pred.Data;
using webapi_pred.Models;
using Microsoft.AspNetCore.Authorization;

namespace webapi_pred.Controllers
{
    // sets api properties and routes
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // gets database context so its available for other sections
        public TeamsController(AppDbContext context)
        {
            _context = context;
        }

        // get method - shows every row after passed by a dto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
        {
            // waits for the db to respond with the teams info
            return await _context.Teams
                // creates the dto so specific information is returned
                .Select(t => new TeamDto
                {
                    TeamId = t.TeamId,
                    Teamname = t.Teamname,
                    LogoUrl = t.LogoPath != null
                        ? $"{Request.Scheme}://{Request.Host}{t.LogoPath}"
                        : null,
                    TotalWins = t.MatchesAsWinner.Count
                })
                .ToListAsync();

        }

        // creates a new row in the table
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<TeamDto>> CreateTeam(Team newTeam)
        {
            // adds the new table
            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();

            // return message of successful creation - uses dto
            return CreatedAtAction(nameof(GetTeamById), new { id = newTeam.TeamId },
                new TeamDto
                {
                    TeamId = newTeam.TeamId,
                    Teamname = newTeam.Teamname,
                    LogoUrl = newTeam.LogoPath != null
                        ? $"{Request.Scheme}://{Request.Host}{newTeam.LogoPath}"
                        : null
                });
        }

        // gets specific row by id - returns dto
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetTeamById(int id)
        {
            // waits for the db to respond if there is a match of id
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();

            // returns the row dto
            return new TeamDto
            {
                TeamId = team.TeamId,
                Teamname = team.Teamname,
                LogoUrl = team.LogoPath != null
                    ? $"{Request.Scheme}://{Request.Host}{team.LogoPath}"
                    : null
            };
        }

        // modifies an existing row
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, TeamDto teamDto)
        {   // looks for id match in the server
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();

            // changes row and saves changes
            team.Teamname = teamDto.Teamname;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // posts a logo
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/upload-logo")]
        public async Task<IActionResult> UploadLogo(int id, IFormFile file)
        {
            // checks if a file exists and if its not empty
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            if (file.Length > 2 * 1024 * 1024) // 2MB max size
                return BadRequest("Maximum file size is 2MB.");

            // checks if there is an id match
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();

            // creates a save path for the file to be saved
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            if (!allowedExtensions.Contains(extension))
                return BadRequest("Invalid file type. Only PNG/JPG/JPEG allowed.");

            if (!string.IsNullOrEmpty(team.LogoPath))
            {
                var oldPath = Path.Combine("wwwroot", team.LogoPath.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            var fileName = $"team-{id}-{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine("wwwroot", "team-logos", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            await using (var stream = new FileStream(filePath, FileMode.Create)) // ensures file is disposed asynchornously
            { // prevents resource leaks
                await file.CopyToAsync(stream);
            }

            team.LogoPath = $"/team-logos/{fileName}";
            await _context.SaveChangesAsync();

            return Ok(new TeamDto
            {
                TeamId = team.TeamId,
                Teamname = team.Teamname,
                LogoUrl = $"{Request.Scheme}://{Request.Host}/team-logos/{fileName}"
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();

            // Delete associated logo file
            if (!string.IsNullOrEmpty(team.LogoPath))
            {
                var logoPath = Path.Combine("wwwroot", team.LogoPath.TrimStart('/'));
                if (System.IO.File.Exists(logoPath))
                {
                    await Task.Run(() => System.IO.File.Delete(logoPath)); // better resource handling
                }
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}