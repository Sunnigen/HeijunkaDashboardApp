namespace HeijunkaAppLibrary.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal TimetoComplete { get; set; }  // minutes
        public string Description { get; set; }
    }
}
