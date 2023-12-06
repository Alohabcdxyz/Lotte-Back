namespace LotteriaView.Models
{
  public class SubmitViewModel
  {
    public CartViewModel CartViewModel { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public int Total { get; set; }
  }
}
