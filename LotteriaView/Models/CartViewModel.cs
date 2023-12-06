using LotteriaAPI.Models;

namespace LotteriaView.Models
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual RegisterViewModel? RegisterViewModel { get; set; }
        public virtual List<CartDetailViewModel>? CartDetails { get; set; }
    }
}
