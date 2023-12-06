namespace LotteriaAPI.Models
{
  public class NewsViewModel
  {
    public string Title { get; set; }
    public IFormFile? ThumbnailImage { get; set; }
    public string Detail { get; set; }
  }
}
