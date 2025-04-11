using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_pred.Data;
using webapi_pred.Models;

namespace webapi_pred.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatchesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
        {
            return await _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Include(m => m.WinnerTeam)
                .Include(m => m.Predictions)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Match>> CreateMatch(Match newMatch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Returns the model validation errors
            }
            _context.Matches.Add(newMatch);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatches), new { id = newMatch.MatchId }, newMatch);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatchById(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Include(m => m.WinnerTeam)
                .Include(m => m.Predictions)
                .FirstOrDefaultAsync(m => m.MatchId == id);

            if (match == null)
            {
                return NotFound();
            }

            return match;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(int id, Match updatedMatch)
        {
            if (id != updatedMatch.MatchId)
            {
                return BadRequest();
            }

            _context.Entry(updatedMatch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Matches.Any(m => m.MatchId == id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
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

    }
}