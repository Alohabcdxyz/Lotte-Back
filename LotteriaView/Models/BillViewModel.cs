using LotteriaView.Models;

namespace LotteriaView.Models
{
  public class BillViewModel
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Total { get; set; }
    public int Status { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string? Note { get; set; }
    public virtual RegisterViewModel? Register { get; set; }
    public virtual List<BillDetailViewModels>? BillDetails { get; set; }
    public DateTime? Created { get; set; }
  }
}
