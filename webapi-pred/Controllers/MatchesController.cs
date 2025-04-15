// using logger for the first time to understand how it works

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_pred.Data;
using SharedDtos;
using webapi_pred.Models;
using Microsoft.AspNetCore.Authorization;

namespace webapi_pred.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MatchesController> _logger;

        public MatchesController(AppDbContext context, ILogger<MatchesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatches(
            [FromQuery] bool? upcomingOnly = null,
            [FromQuery] bool? completedOnly = null)
        {
            try
            {
                var query = _context.Matches
                    .Include(m => m.Team1)
                    .Include(m => m.Team2)
                    .Include(m => m.WinnerTeam)
                    .AsQueryable();

                if (upcomingOnly == true)
                {
                    query = query.Where(m => m.MatchDate > DateTime.UtcNow);
                }
                else if (completedOnly == true)
                {
                    query = query.Where(m => m.WinnerTeamId != null);
                }

                var matches = await query
                    .OrderBy(m => m.MatchDate)
                    .ToListAsync();

                var result = matches.Select(m => MapToDto(m, Request)).ToList();
                return Ok(matches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting matches");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatchDto>> GetMatch(int id)
        {
            try
            {
                var match = await _context.Matches
                    .Include(m => m.Team1)
                    .Include(m => m.Team2)
                    .Include(m => m.WinnerTeam)
                    .FirstOrDefaultAsync(m => m.MatchId == id);

                if (match == null)
                {
                    return NotFound();
                }

                return Ok(match);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting match with ID {id}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MatchDto>> CreateMatch(CreateMatchDto createMatchDto)
        {
            try
            {
                var team1 = await _context.Teams.FindAsync(createMatchDto.Team1Id);
                var team2 = await _context.Teams.FindAsync(createMatchDto.Team2Id);

                if (team1 == null || team2 == null)
                {
                    return BadRequest("One or both teams do not exist.");
                }

                if (createMatchDto.Team1Id == createMatchDto.Team2Id)
                {
                    return BadRequest("A team cannot play against itself.");
                }

                if (createMatchDto.MatchDate <= DateTime.UtcNow)
                {
                    return BadRequest("Match date must be in the future.");
                }

                var match = new Match
                {
                    Team1Id = createMatchDto.Team1Id,
                    Team2Id = createMatchDto.Team2Id,
                    Team1 = team1,
                    Team2 = team2,
                    MatchDate = createMatchDto.MatchDate,
                    Team1Score = 0,
                    Team2Score = 0
                };

                _context.Matches.Add(match);
                await _context.SaveChangesAsync();

                var newMatch = await _context.Matches
                    .Include(m => m.Team1)
                    .Include(m => m.Team2)
                    .FirstOrDefaultAsync(m => m.MatchId == match.MatchId);

                return CreatedAtAction(nameof(GetMatch), new { id = match.MatchId }, MapToDto(newMatch!, Request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating match");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/result")]
        public async Task<IActionResult> UpdateMatchResult(int id, UpdateMatchDto updateDto)
        {
            try
            {
                var match = await _context.Matches
                    .Include(m => m.Team1)
                    .Include(m => m.Team2)
                    .FirstOrDefaultAsync(m => m.MatchId == id);

                if (match == null)
                {
                    return NotFound();
                }

                if (match.MatchDate > DateTime.UtcNow)
                {
                    return BadRequest("Cannot update result for future matches.");
                }

                if (updateDto.WinnerTeamId != null &&
                    updateDto.WinnerTeamId != match.Team1Id &&
                    updateDto.WinnerTeamId != match.Team2Id)
                {
                    return BadRequest("Winner must be one of the competing teams");
                }

                match.Team1Score = updateDto.Team1Score;
                match.Team2Score = updateDto.Team2Score;
                match.WinnerTeamId = updateDto.WinnerTeamId;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating match result for ID {id}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            try
            {
                var match = await _context.Matches.FindAsync(id);
                if (match == null)
                {
                    return NotFound();
                }

                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting match with ID {id}");
                return StatusCode(500, "An error ocurred while processing your request.");
            }
        }

        public static MatchDto MappingHelper(Match m, HttpRequest request)
        {
            return MapToDto(m, request);
        }

        private static MatchDto MapToDto(Match m, HttpRequest request)
        {
            return new MatchDto
            {
                MatchId = m.MatchId,
                Team1Id = m.Team1Id,
                Team2Id = m.Team2Id,
                Team1 = new TeamDto
                {
                    TeamId = m.Team1.TeamId,
                    Teamname = m.Team1.Teamname,
                    LogoUrl = m.Team1.LogoPath != null
                        ? $"{request.Scheme}://{request.Host}{m.Team1.LogoPath}"
                        : null
                },
                Team2 = new TeamDto
                {
                    TeamId = m.Team2.TeamId,
                    Teamname = m.Team2.Teamname,
                    LogoUrl = m.Team2.LogoPath != null
                        ? $"{request.Scheme}://{request.Host}{m.Team2.LogoPath}"
                        : null
                },
                Team1Score = m.Team1Score,
                Team2Score = m.Team2Score,
                WinnerTeam = m.WinnerTeam != null ? new TeamDto
                {
                    TeamId = m.WinnerTeam.TeamId,
                    Teamname = m.WinnerTeam.Teamname,
                    LogoUrl = m.WinnerTeam.LogoPath != null
                        ? $"{request.Scheme}://{request.Host}{m.WinnerTeam.LogoPath}"
                        : null
                } : null,
                MatchDate = m.MatchDate,
                IsCompleted = m.WinnerTeamId != null,
            };
        }
    }
}