namespace LotteriaAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual Register? Register { get; set; }
        public virtual List<CartDetail>? CartDetails { get; set; }
    }
}
