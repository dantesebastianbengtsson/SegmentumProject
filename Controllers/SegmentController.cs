using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Segmentum.Data;
using Segmentum.Models;

namespace Segmentum.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SegmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SegmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSegments()
        {
            var segments = await _context.Segments.ToListAsync();
            Console.WriteLine($"Fetched {segments.Count} segments from the database.");
            return Ok(segments);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("SegmentController is working!");
        }

        [HttpPost]
        public async Task<IActionResult> CreateSegment(Segment segment)
        {
            if (segment == null)
            {
                return BadRequest("Segment data is null.");
            }

            _context.Segments.Add(segment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSegments), new { id = segment.Id }, segment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSegment(int id)
        {
            var segment = await _context.Segments.FindAsync(id);
            if (segment == null)
            {
                return NotFound($"No segment found with ID {id}.");
            }

            _context.Segments.Remove(segment);
            await _context.SaveChangesAsync();

            return Ok($"Segment with ID {id} has been deleted.");
        }

    }

}
