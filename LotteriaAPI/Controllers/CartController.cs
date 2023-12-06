using LotteriaAPI.Data;
using LotteriaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LotteriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly LotteDbContext _lotteDbContext;
        public CartController(LotteDbContext lotteDbContext)
        {
            _lotteDbContext = lotteDbContext;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(int userId,[FromBody] CartDetail test)
        {
            var product = await _lotteDbContext.Products
                .FirstOrDefaultAsync(p => p.Id == test.ProductId);

            if (product == null)
            {
                return BadRequest("Product not found");
            }

            var cart = await _lotteDbContext.Carts
                .Include(c => c.CartDetails)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _lotteDbContext.Carts.Add(cart); await _lotteDbContext.SaveChangesAsync();
            }

            var cartDetail = _lotteDbContext.CartDetails.FirstOrDefault(cd => cd.ProductId == test.ProductId && cd.CartId == cart.Id);

            if (cartDetail != null)
            {
                // Update quantity or perform other actions
                cartDetail.Quantity += test.Quantity;
            }
            else
            {
                cartDetail = new CartDetail
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = test.Quantity,
                    Price = product.Price
                };
                _lotteDbContext.CartDetails.Add(cartDetail);
            }

            await _lotteDbContext.SaveChangesAsync();

            return Ok("Item added to the cart");

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCart(int id)
        {
            var cart = await _lotteDbContext.Carts.Include(x => x.CartDetails).Include(y => y.Register).FirstOrDefaultAsync(x => x.UserId == id);
            return Ok(cart);
        }

        [HttpGet]
        public async Task<IActionResult> GetCartDetail(int id)
        {
            var cart = await _lotteDbContext.CartDetails.Where(x => x.CartId == id).ToListAsync();
            return Ok(cart);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCartItem(int userId, int productId)
        {
            try
            {

                var cart = await _lotteDbContext.Carts
                    .Include(c => c.CartDetails)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    return NotFound("Cart not found");
                }

                var cartItem = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == productId);

                if (cartItem == null)
                {
                    return NotFound("Cart item not found");
                }

                _lotteDbContext.CartDetails.Remove(cartItem);
                await _lotteDbContext.SaveChangesAsync();

                return Ok("Cart item deleted");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    [HttpDelete]
    public async Task<IActionResult> RemoveCart(int userId, int cartId)
    {
      try
      {

        var cart = await _lotteDbContext.Carts
            .Include(c => c.CartDetails)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
          return NotFound("Cart not found");
        }

        var cartItem = cart.CartDetails.Where(cd => cd.CartId == cartId);

        if (cartItem == null)
        {
          return NotFound("Cart item not found");
        }

        _lotteDbContext.CartDetails.RemoveRange(cart.CartDetails);
        await _lotteDbContext.SaveChangesAsync();

        return Ok("Cart item deleted");
      }
      catch (Exception ex)
      {
        return BadRequest($"Error: {ex.Message}");
      }
    }

    [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int userId, int productId)
        {
            var cart = await _lotteDbContext.Carts
                  .Include(c => c.CartDetails)
                  .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            var cartItem = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
                _lotteDbContext.SaveChanges();
            }
            return Ok("Them thanh cong");
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int userId, int productId)
        {

            var cart = await _lotteDbContext.Carts
                  .Include(c => c.CartDetails)
                  .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            var cartItem = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == productId);

            if (cartItem != null)
            {
        cartItem.Quantity--;
        if (cartItem.Quantity == 0) {
            _lotteDbContext.CartDetails.Remove(cartItem);
        }

        _lotteDbContext.SaveChanges();
            }
            return Ok("Giam thanh cong");
        }

    }
}
