namespace LotteriaView.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Infor { get; set; }
        public string ProductString { get; set; }
        public int? CategoryId { get; set; }
    public virtual CategoryViewModel? Category { get; set; }
    public virtual List<CartDetailViewModel>? CartDetails { get; set; }

    }
}
