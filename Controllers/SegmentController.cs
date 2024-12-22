using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Segmentum.Data;

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
    }

}
