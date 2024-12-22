using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Segmentum.Data;
using Segmentum.Models;

namespace Segmentum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HabitController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/habit
        [HttpGet]
        public async Task<IActionResult> GetHabits()
        {
            var habits = await _context.Habits.ToListAsync();
            return Ok(habits);
        }

        // GET: api/habit/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHabit(int id)
        {
            var habit = await _context.Habits.FindAsync(id);
            if (habit == null)
            {
                return NotFound();
            }

            return Ok(habit);
        }

        // POST: api/habit
        [HttpPost]
        public async Task<IActionResult> CreateHabit(Habit habit)
        {
            _context.Habits.Add(habit);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetHabit), new { id = habit.Id }, habit);
        }

        // PUT: api/habit/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabit(int id, Habit updatedHabit)
        {
            if (id != updatedHabit.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedHabit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Habits.Any(h => h.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/habit/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabit(int id)
        {
            var habit = await _context.Habits.FindAsync(id);
            if (habit == null)
            {
                return NotFound();
            }

            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
