namespace LotteriaView.Models
{
    public class CartDetailViewModel
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public virtual ProductViewModel? ProductViewModel  { get; set; }
        public virtual CartViewModel? CartViewModel { get; set; }
    }
}
