using LotteriaAPI.Data;
using LotteriaAPI.Migrations;
using LotteriaAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LotteriaAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]/[action]")]
  public class BillController : Controller
  {
    private readonly LotteDbContext _lotteDbContext;
    public BillController(LotteDbContext lotteDbContext)
    {
      _lotteDbContext = lotteDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBill()
    {
      var bill = await _lotteDbContext.Bills.Include(x => x.Register).Include(x => x.BillDetails).ThenInclude(bd => bd.Product).ToListAsync();
      return Ok(bill) ;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllBillByUser(int id)
    {
      var bill = await _lotteDbContext.Bills.Include(x => x.BillDetails).Include(x => x.Register).Where(x => x.UserId == id).ToListAsync();
      return Ok(bill);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBill(int id)
    {
      var cart = (await _lotteDbContext.Bills.OrderByDescending(x => x.Created).FirstOrDefaultAsync(x => x.UserId == id));
      return Ok(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToBill([FromBody] Bill bill) {

      bill.Created = DateTime.Now;
      await _lotteDbContext.Bills.AddAsync(bill);
      await _lotteDbContext.SaveChangesAsync();
      return Ok("OK");

    }

    [HttpPost]
    public async Task<IActionResult> AddToBillDetail([FromBody] BillDetail billDetail)
    {

      await _lotteDbContext.BillDetails.AddAsync(billDetail);
      await _lotteDbContext.SaveChangesAsync();
      return Ok("OK");

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> CancelOrder(CancelBill bill, int id)
    {
      // Find the order by its ID
      var order = _lotteDbContext.Bills.Find(id);

      if (order == null)
      {
        return NotFound(); // Return a 404 Not Found response if the order is not found
      }

      if (order.Status == 0)
      {
        // Set the order status to 3 (Cancelled)
        order.Status = 3;
        order.Note = bill.Note;
        _lotteDbContext.SaveChanges(); // Save changes to the database

        return Ok(); // Return a 200 OK response to indicate a successful cancellation
      }
      else
      {
        return BadRequest("Unable to cancel order. The order is not in a cancellable state."); // Return a 400 Bad Request response if the order can't be cancelled
      }
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> ConfirmOrder(int id)
    {
      
      var order = _lotteDbContext.Bills.Include(x => x.BillDetails).FirstOrDefault(x => x.Id == id);

      if (order == null)
      {
        return NotFound(); 
      }

      if (order.Status == 0)
      {

        foreach (var item in order.BillDetails)
        {
          var prd = _lotteDbContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
          prd.Quantity = prd.Quantity - item.Quantity;
          _lotteDbContext.Update(prd);
        }
        order.Status = 1;
        _lotteDbContext.SaveChanges();

        return Ok(); 
      }
      else
      {
        return BadRequest("Unable to cancel order. The order is not in a cancellable state."); 
      }
    }


    [HttpPost]
    public async Task<IActionResult> DoneOrder(int id)
    {
      
      var order = _lotteDbContext.Bills.Find(id);

      if (order == null)
      {
        return NotFound(); 
      }

      if (order.Status == 1)
      {
      
        order.Status = 2;
        _lotteDbContext.SaveChanges(); 

        return Ok(); 
      }
      else
      {
        return BadRequest("Unable to cancel order. The order is not in a cancellable state."); 
      }
    }
  }
}
