namespace Segmentum.Models
{
    public class Segment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Habit>? Habits { get; set; }
    }
}
