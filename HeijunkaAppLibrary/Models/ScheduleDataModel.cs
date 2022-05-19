using System;

namespace HeijunkaAppLibrary.Models
{
    public class ScheduleDataModel
    {
        public int Id { get; set; }
        public int QueueId { get; set; }
        public int UserLastModifiedId { get; set; }
        public int ProcessId { get; set; }
        public string Subject { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndDate { get; private set; }
        public DateTime EndTime { get; set; }
        public bool IsComplete { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public void SetEndDate(double duration)
        {
            // Update EndDate
            EndDate = StartDate.AddMinutes(duration);
        }
    }
}
