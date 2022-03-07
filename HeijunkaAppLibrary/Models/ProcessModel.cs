namespace HeijunkaAppLibrary.Models
{
    public class ProcessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Duration { get; set; }  // minutes
        public string Description { get; set; }
    }
}
