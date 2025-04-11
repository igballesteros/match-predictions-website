using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_pred.Data;
using webapi_pred.Models;

namespace webapi_pred.Controllers
{
    [Route("api/[controller]")]
    public class PredictionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PredictionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prediction>>> GetPredictions()
        {
            return await _context.Predictions
                .Include(p => p.User)
                .Include(p => p.Match)
                    .ThenInclude(m => m.Team1)
                .Include(p => p.Match)
                    .ThenInclude(m => m.Team2)
                .Include(p => p.PredictedWinner)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Prediction>> CreatePrediction()
        {
            // Read the request body as a string FIRST
            string rawJson;
            using (var reader = new StreamReader(Request.Body))
            {
                rawJson = await reader.ReadToEndAsync();
                Console.WriteLine($"Raw request body: {rawJson}");
            }

            // Manually deserialize (to avoid stream issues)
            var newPrediction = JsonSerializer.Deserialize<Prediction>(rawJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ensure camelCase -> PascalCase mapping
            });

            if (newPrediction == null)
            {
                return BadRequest("Invalid JSON format.");
            }

            var matchExists = await _context.Matches.AnyAsync(m => m.MatchId == newPrediction.MatchId);

            if (!matchExists)
            {
                return BadRequest($"Match with ID {newPrediction.MatchId} does not exist.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.UserId == newPrediction.UserId);

            if (!userExists)
            {
                return BadRequest($"User with ID {newPrediction.UserId} does not exist.");
            }

            // Save to DB
            _context.Predictions.Add(newPrediction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPredictions), new { id = newPrediction.PredictionId }, newPrediction);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Prediction>> GetPrediction(int id)
        {
            var prediction = await _context.Predictions
                            .Include(p => p.User)
                            .Include(p => p.Match)
                                .ThenInclude(m => m.Team1)
                            .Include(p => p.Match)
                                .ThenInclude(m => m.Team2)
                            .Include(p => p.PredictedWinner)
                            .FirstOrDefaultAsync(p => p.PredictionId == id);

            if (prediction == null)
            {
                return NotFound();
            }

            return prediction;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrediction(int id, Prediction updatedPrediction)
        {
            if (id != updatedPrediction.PredictionId)
            {
                return BadRequest();
            }

            _context.Entry(updatedPrediction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Predictions.Any(p => p.PredictionId == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        public async Task<IActionResult> DeletePrediction(int id)
        {
            var prediction = await _context.Predictions.FindAsync(id);
            if (prediction == null)
            {
                return NotFound();
            }

            _context.Predictions.Remove(prediction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}