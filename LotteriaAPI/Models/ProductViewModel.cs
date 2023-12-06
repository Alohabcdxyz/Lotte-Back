namespace LotteriaAPI.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Infor { get; set; }
    public string Color { get; set; }
    public IFormFile? ProductImage { get; set; }
        public int? CategoryId { get; set; }
    }
}
