using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LotteriaAPI.Data;
using LotteriaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LotteriaAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly LotteDbContext _lotteDbContext;
        public ProductController(LotteDbContext lotteDbContext)
        {
            _lotteDbContext = lotteDbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_lotteDbContext.Products == null)
            {
                return NotFound();
            }
            return await _lotteDbContext.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_lotteDbContext.Products == null)
            {
                return NotFound();
            }
            var product = await _lotteDbContext.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromForm] ProductViewModel _product)
        {
            var product = await _lotteDbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = _product.Name;
            product.CategoryId = _product.CategoryId;
            product.Infor = _product.Infor;
            product.Quantity = _product.Quantity;
            product.Price = _product.Price;
      product.Color = _product.Color;
            if (_product.ProductImage != null)
            {
                product.ProductString = await ConvertImageBase.ImageToBase64(_product.ProductImage);
            }

            await _lotteDbContext.SaveChangesAsync();
            return Ok(product);
        }

        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductViewModel _product)
        {
            var product = new Product
            {
                Name = _product.Name,
                Infor = _product.Infor,
                Quantity = _product.Quantity,
                Price = _product.Price,
                Color = _product.Color,
                CategoryId = _product.CategoryId,
            };

            product.ProductString = await ConvertImageBase.ImageToBase64(_product.ProductImage);

            await _lotteDbContext.Products.AddAsync(product);
            await _lotteDbContext.SaveChangesAsync();
            return Ok(_product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_lotteDbContext.Products == null)
            {
                return NotFound();
            }
            var product = await _lotteDbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _lotteDbContext.Products.Remove(product);
            await _lotteDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
