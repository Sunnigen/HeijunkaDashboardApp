using HeijunkaFrontEnd.Models;

namespace HeijunkaTest.Models
{
    public class AppointmentData
    {
        private DateTime startTime { get; set; }
        private PartModel partModel { get; set; }
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string? Subject { get; set; }
        public string OrderNumber { get; set; }
        public PartModel PartModel
        {
            get { return partModel; }
            set 
            {
                partModel = value;
                Subject = value.Name;
            }
        }
        public DateTime StartTime 
        {
            get { return startTime; }
            set 
            { 
                startTime = value;
                EndTime = startTime.AddMinutes((double)PartModel.Duration);
            } 
        }
        public DateTime EndTime { get; private set; }
        
    }
}
