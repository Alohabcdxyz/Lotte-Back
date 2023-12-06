namespace LotteriaAPI.Models
{
    public class BillDetail
    {
      public int Quantity { get; set; }
      public double Price { get; set; }
      public int BillId { get; set; }
      public int ProductId { get; set; }
      public virtual Bill? Bill { get; set; }
      public virtual Product? Product { get; set; }
  }
}
