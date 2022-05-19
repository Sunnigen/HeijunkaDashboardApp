using System;
namespace HeijunkaAppLibrary.Models
{
    public class QueueModel
    {
        public int Id { get; set; }
        public string QueueName { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserLastModifiedId { get; set; }
        public DateTime UserLastModifiedDate { get; set; }
        public bool IsActive { get; set; } = false;
        public string ClassName { get; set; } = "e-child-node";
        public string Color { get; set; }
    }
}
