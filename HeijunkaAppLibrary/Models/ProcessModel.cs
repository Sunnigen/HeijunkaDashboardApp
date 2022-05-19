namespace HeijunkaAppLibrary.Models
{
    public class ProcessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Duration { get; set; } = 10.0; // minutes
        public string Description { get; set; }
    }
}
