namespace HeijunkaTest.Models
{
    public class AppointmentData
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
