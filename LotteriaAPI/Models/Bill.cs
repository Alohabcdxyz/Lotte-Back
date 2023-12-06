using System.Text.Json.Serialization;

namespace LotteriaAPI.Models
{
    public class Bill
    {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Total { get; set; }
    public int Status { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string? Note { get; set; }
 
    public virtual Register? Register { get; set; }

    public virtual List<BillDetail>? BillDetails { get; set; }
    public DateTime? Created { get; set; }
  }
}
