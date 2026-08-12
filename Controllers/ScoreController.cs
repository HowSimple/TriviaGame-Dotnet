using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriviaApp.Contexts;
using TriviaApp.Models;

namespace TriviaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Policy1")]
    public class ScoresController : ControllerBase
    {
        private readonly TriviaContext _context;

        public ScoresController(TriviaContext context)
        {
            _context = context;
        }

        // POST api/Scores
        // Saves (or updates) the high score for the authenticated user.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Score>> SaveScore([FromBody] SaveScoreRequest request)
        {
            // Auth0 puts the user's unique ID in the "sub" claim.
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Could not determine user identity from token.");

            // Upsert: keep only the best score per user.
            var existing = await _context.scores
                .FirstOrDefaultAsync(h => h.UserId == userId);

            if (existing == null)
            {
                var newScore = new Score
                {
                    UserId = userId,
                    UserName = request.UserName,
                    Value = request.Score,
                };
                _context.scores.Add(newScore);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetMyScore), newScore);
            }

            // Only update if the new score is higher.
            if (request.Score > existing.Value)
            {
                existing.Value = request.Score;
                existing.UserName = request.UserName ?? existing.UserName;
                existing.UpdateLastModified();
                await _context.SaveChangesAsync();
            }

            return Ok(existing);
        }

        // GET api/Scores/me — returns the caller's own score
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<Score>> GetMyScore()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var score = await _context.scores
                .FirstOrDefaultAsync(h => h.UserId == userId);

            if (score == null)
                return NotFound();

            return Ok(score);
        }

        // GET api/Scores/leaderboard — top 10, no auth required
        [HttpGet("leaderboard")]
        public async Task<ActionResult<IEnumerable<Score>>> GetLeaderboard()
        {
            var top10 = await _context.scores
                .OrderByDescending(h => h.Value)
                .Take(10)
                .ToListAsync();

            return Ok(top10);
        }
    }

    public class SaveScoreRequest
    {
        public int Score { get; set; }
        public string? UserName { get; set; }
    }
}