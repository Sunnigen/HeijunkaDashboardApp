namespace HeijunkaFrontEnd.Models
{
    public class StagingObjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasChild { get; set; } = false;
        public int PId { get; set; }
        public bool Expanded { get; set; } = false;


    }
}
