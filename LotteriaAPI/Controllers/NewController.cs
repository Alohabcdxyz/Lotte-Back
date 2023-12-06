using LotteriaAPI.Data;
using LotteriaAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LotteriaAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]/[action]")]
  public class NewController : Controller
  {
    private readonly LotteDbContext _lotteDbContext;
    public NewController(LotteDbContext lotteDbContext)
    {
      _lotteDbContext = lotteDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNews()
    {
      var news = await _lotteDbContext.News.ToListAsync();
      return Ok(news);
    }

    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> AddNews([FromForm] NewsViewModel _news)
    {
      var news = new News
      {
        Title = _news.Title,
        Detail = _news.Detail,
      };

      news.Thumbnail = await ConvertImageBase.ImageToBase64(_news.ThumbnailImage);

      await _lotteDbContext.News.AddAsync(news);
      await _lotteDbContext.SaveChangesAsync();
      return Ok(news);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetNews([FromRoute] int id)
    {
      var news = await _lotteDbContext.News.FirstOrDefaultAsync(x => x.Id == id);
      if (news == null)
      {
        return NotFound();
      }
      return Ok(news);
    }

    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateNews([FromRoute] int id, [FromForm] NewsViewModel _news)
    {
      var news = await _lotteDbContext.News.FindAsync(id);
      if (news == null)
      {
        return NotFound();
      }
      news.Title = _news.Title;
      news.Detail = _news.Detail;
      if (_news.ThumbnailImage != null)
      {
        news.Thumbnail = await ConvertImageBase.ImageToBase64(_news.ThumbnailImage);
      }

      await _lotteDbContext.SaveChangesAsync();
      return Ok(news);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteNews([FromRoute] int id)
    {
      var news = await _lotteDbContext.News.FindAsync(id);
      if (news == null)
      {
        return NotFound();
      }
      _lotteDbContext.News.Remove(news);
      await _lotteDbContext.SaveChangesAsync();
      return Ok(news);
    }
  }
}
