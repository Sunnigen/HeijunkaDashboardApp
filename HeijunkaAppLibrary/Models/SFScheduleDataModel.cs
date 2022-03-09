using System;
using System.Collections.Generic;
using System.Text;

namespace HeijunkaAppLibrary.Models
{
    public class SFScheduleDataModel
    {
        public string Subject { get; set; }
        public string OrderNumber { get; set; }
        public int QueueId { get; set; }

        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public string? StartTimezone { get; set; }
        public string? EndTimezone { get; set; }
        public string? RecurrenceRule { get; set; }
        public string? RecurrenceException { get; set; }
        public int? RecurrenceID { get; set; }
    }
}
