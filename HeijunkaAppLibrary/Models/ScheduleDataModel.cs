using System;

namespace HeijunkaAppLibrary.Models
{
    public class ScheduleDataModel
    {
        private DateTime startDate { get; set; }

        public int Id { get; set; }
        public int QueueId { get; set; }
        public int UserLastModifiedId { get; set; }
        public int ProcessId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                // Update EndDate
                EndDate = startDate.AddMinutes((double)TimetoComplete);
            } 
        }
        public DateTime EndDate { get; private set; }
        public bool IsComplete { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public decimal TimetoComplete{ get; set; }  // in minutes
    }
}
