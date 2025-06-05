namespace TakeHomeChallenge.Models
{
    public class TimeEntry
    {
        public int EntryId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan InTime { get; set; }
        public TimeSpan OutTime { get; set; }
    }
}
