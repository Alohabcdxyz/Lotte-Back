namespace LotteriaAPI.Models
{
    public class CartDetail
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual Product? Product { get; set; }
    }
}
