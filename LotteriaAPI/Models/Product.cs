namespace LotteriaAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Infor { get; set; }
    public string Color { get; set; }
    public string ProductString { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual List<CartDetail>? CartDetails { get; set; }
        public virtual List<BillDetail>? BillDetails { get; set; }
  }
}
