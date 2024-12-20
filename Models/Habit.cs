namespace Segmentum.Models
{
    public class Habit
    { 
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Status { get; set; } // e.g., "Succeeded", "Skipped", "Failed"
        public int SegmentId { get; set; }
        public Segment? Segment { get; set; }
    }
}
