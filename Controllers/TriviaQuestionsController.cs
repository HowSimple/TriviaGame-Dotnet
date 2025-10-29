using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriviaApp.Contexts;
using TriviaApp.Models;

namespace TriviaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriviaQuestionsController : ControllerBase
    {
        private readonly TriviaContext _context;

        public TriviaQuestionsController(TriviaContext context)
        {
            _context = context;
        }

        // GET: api/TriviaQuestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TriviaQuestion>>> GettriviaQuestions()
        {
            return await _context.triviaQuestions.ToListAsync();
        }

        // GET: api/TriviaQuestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TriviaQuestion>> GetTriviaQuestion(Guid id)
        {
            var triviaQuestion = await _context.triviaQuestions.FindAsync(id);

            if (triviaQuestion == null)
            {
                return NotFound();
            }

            return triviaQuestion;
        }

        // PUT: api/TriviaQuestions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTriviaQuestion(Guid id, TriviaQuestion triviaQuestion)
        {
            if (id != triviaQuestion.Id)
            {
                return BadRequest();
            }

            _context.Entry(triviaQuestion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TriviaQuestionExists(id))
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

        // POST: api/TriviaQuestions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TriviaQuestion>> PostTriviaQuestion(TriviaQuestion triviaQuestion)
        {
            _context.triviaQuestions.Add(triviaQuestion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTriviaQuestion", new { id = triviaQuestion.Id }, triviaQuestion);
        }

        // DELETE: api/TriviaQuestions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTriviaQuestion(Guid id)
        {
            var triviaQuestion = await _context.triviaQuestions.FindAsync(id);
            if (triviaQuestion == null)
            {
                return NotFound();
            }

            _context.triviaQuestions.Remove(triviaQuestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TriviaQuestionExists(Guid id)
        {
            return _context.triviaQuestions.Any(e => e.Id == id);
        }
    }
}
