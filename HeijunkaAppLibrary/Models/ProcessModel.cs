using System;
using System.Collections.Generic;
using System.Text;

namespace HeijunkaAppLibrary.Models
{
    public class ProcessModel
    {
        public int Id { get; set; }
        public int QueueId { get; set; }
        public int UserLastModifiedId { get; set; }
        public int ProductId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime StartDate
        {
            get { return StartDate; }
            set
            {
                StartDate = value;
                // Update EndDate
                EndDate = StartDate.AddMinutes((double)TimetoComplete);
            } 
        }
        public DateTime EndDate { get; private set; }
        public bool IsComplete { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
        public decimal TimetoComplete{ get; set; }  // in minutes
    }
}
