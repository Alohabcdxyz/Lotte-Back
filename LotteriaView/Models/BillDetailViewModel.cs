using LotteriaAPI.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace LotteriaView.Models
{
  public class BillDetailViewModels
  {
    public int Quantity { get; set; }
    public double Price { get; set; }
    public int BillId { get; set; }
    public int ProductId { get; set; }
    public string ProductString { get; set; }

  }
}
