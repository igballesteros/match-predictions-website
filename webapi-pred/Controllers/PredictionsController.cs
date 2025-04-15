using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_pred.Data;
using SharedDtos;
using webapi_pred.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace webapi_pred.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

    public class PredictionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PredictionsController> _logger;

        public PredictionsController(AppDbContext context, ILogger<PredictionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PredictionDto>>> GetPredictions(
            [FromQuery] int? userId = null,
            [FromQuery] int? matchId = null)
        {
            try
            {
                var query = _context.Predictions
                    .Include(p => p.User)
                    .Include(p => p.Match)
                        .ThenInclude(m => m.Team1)
                    .Include(p => p.Match)
                        .ThenInclude(m => m.Team2)
                    .Include(p => p.Match)
                        .ThenInclude(m => m.WinnerTeam)
                    .Include(p => p.PredictedWinner)
                    .AsQueryable();

                // Apply filters
                if (userId.HasValue)
                {
                    query = query.Where(p => p.UserId == userId.Value);
                }

                if (matchId.HasValue)
                {
                    query = query.Where(p => p.MatchId == matchId.Value);
                }

                var predictions = await query.ToListAsync();
                return Ok(predictions.Select(p => MapToDto(p, Request)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting predictions");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PredictionDto>> GetPrediction(int id)
        {
            try
            {
                var prediction = await _context.Predictions
                    .Include(p => p.User)
                    .Include(p => p.Match)
                        .ThenInclude(m => m.Team1)
                    .Include(p => p.Match)
                        .ThenInclude(m => m.Team2)
                    .Include(p => p.Match)
                        .ThenInclude(m => m.WinnerTeam)
                    .Include(p => p.PredictedWinner)
                    .FirstOrDefaultAsync(p => p.PredictionId == id);

                if (prediction == null)
                {
                    return NotFound();
                }

                return Ok(MapToDto(prediction, Request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting prediction with ID {id}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PredictionDto>> CreatePrediction(CreatePredictionDto createDto)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var match = await _context.Matches.FindAsync(createDto.MatchId);

                if (match == null)
                {
                    return BadRequest("Match does not exist.");
                }

                if (match.MatchDate <= DateTime.UtcNow)
                {
                    return BadRequest("Cannot predict on completed or ongoing matches.");
                }

                if (createDto.PredictedWinnerId != match.Team1Id &&
                    createDto.PredictedWinnerId != match.Team2Id)
                {
                    return BadRequest("Predicted winner must be one of the competing teams");
                }

                var existingPrediction = await _context.Predictions
                    .FirstOrDefaultAsync(p => p.UserId == currentUserId && p.MatchId == createDto.MatchId);

                if (existingPrediction != null)
                {
                    return BadRequest("You have already predicted this match");
                }

                var prediction = new Prediction
                {
                    UserId = currentUserId,
                    MatchId = createDto.MatchId,
                    PredictedWinnerId = createDto.PredictedWinnerId,
                    PredictedTeam1Score = createDto.PredictedTeam1Score,
                    PredictedTeam2Score = createDto.PredictedTeam2Score
                };

                _context.Predictions.Add(prediction);
                await _context.SaveChangesAsync();

                // Reload related data
                var newPrediction = await _context.Predictions
                    .Include(p => p.Match)
                    .Include(p => p.PredictedWinner)
                    .FirstOrDefaultAsync(p => p.PredictionId == prediction.PredictionId);

                return CreatedAtAction(nameof(GetPrediction),
                    new { id = prediction.PredictionId },
                    MapToDto(newPrediction!, Request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating prediction.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrediction(int id, UpdatePredictionDto updateDto)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var prediction = await _context.Predictions
                    .Include(p => p.Match)
                    .FirstOrDefaultAsync(p => p.PredictionId == id);

                if (prediction == null)
                {
                    return NotFound();
                }

                if (prediction.UserId != currentUserId)
                {
                    return Forbid();
                }

                if (prediction.Match.MatchDate <= DateTime.UtcNow)
                {
                    return BadRequest("Cannot update prediction for completed or ongoing matches");
                }

                if (updateDto.PredictedWinnerId != prediction.Match.Team1Id &&
                    updateDto.PredictedWinnerId != prediction.Match.Team2Id)
                {
                    return BadRequest("Predicted winner must be one of the competing teams");
                }

                prediction.PredictedWinnerId = updateDto.PredictedWinnerId;
                prediction.PredictedTeam1Score = updateDto.PredictedTeam1Score;
                prediction.PredictedTeam2Score = updateDto.PredictedTeam2Score;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating prediction with ID {id}");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrediction(int id)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var prediction = await _context.Predictions
                    .Include(p => p.Match)
                    .FirstOrDefaultAsync(p => p.PredictionId == id);

                if (prediction == null)
                {
                    return NotFound();
                }

                // Validate ownership
                if (prediction.UserId != currentUserId)
                {
                    return Forbid();
                }

                // Validate match is still upcoming
                if (prediction.Match.MatchDate <= DateTime.UtcNow)
                {
                    return BadRequest("Cannot delete prediction for completed or ongoing matches");
                }

                _context.Predictions.Remove(prediction);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting prediction with ID {id}");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        private static PredictionDto MapToDto(Prediction p, HttpRequest request)
        {
            return new PredictionDto
            {
                PredictionId = p.PredictionId,
                UserId = p.UserId,
                MatchId = p.MatchId,
                Match = p.Match != null ? MatchesController.MappingHelper(p.Match, request) : null!,
                PredictedWinnerId = p.PredictedWinnerId,
                PredictedWinner = p.PredictedWinner != null ? new TeamDto
                {
                    TeamId = p.PredictedWinner.TeamId,
                    Teamname = p.PredictedWinner.Teamname,
                    LogoUrl = !string.IsNullOrEmpty(p.PredictedWinner.LogoPath)
                        ? $"{request.Scheme}://{request.Host}{p.PredictedWinner.LogoPath}"
                        : null
                } : null!,
                PredictedTeam1Score = p.PredictedTeam1Score,
                PredictedTeam2Score = p.PredictedTeam2Score
            };
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning($"Invalid user claim attempt: {userIdClaim}");
                throw new UnauthorizedAccessException("Invalid user identifier in token");
            }

            return userId;
        }
    }
}