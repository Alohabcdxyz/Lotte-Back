using LotteriaAPI.Data;
using LotteriaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LotteriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RegisterController : Controller
    {
        private readonly LotteDbContext _lotteDbContext;
        public RegisterController(LotteDbContext lotteDbContext)
        {
            _lotteDbContext = lotteDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _lotteDbContext.Register.ToListAsync());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetDetailUser([FromRoute] int id)
        {
            var contact = await _lotteDbContext.Register.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(Register register)
        {

            if (ModelState.IsValid)
            {
              
                _lotteDbContext.Register.Add(register);
                await _lotteDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetUser));
            }
            return View(register);

        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateContact([FromRoute] int id, Register _contact)
        {
            var contact = await _lotteDbContext.Register.FindAsync(id);
            if (contact != null)
            {
                contact.UserName = _contact.UserName;
                contact.Email = _contact.Email;
                contact.Password = _contact.Password;
                contact.ConfirmPassword = _contact.ConfirmPassword;

                await _lotteDbContext.SaveChangesAsync();

                return Ok(contact);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteContact([FromRoute] int id)
        {
            var contact = await _lotteDbContext.Register.FindAsync(id);
            if (contact != null)
            {
                _lotteDbContext.Remove(contact);
                await _lotteDbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {

            if (ModelState.IsValid)
            {

                var login = await _lotteDbContext.Register.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (login != null && login.Password == user.Password)
                {

                    return Ok(login);
                }
                else
                {
                    ModelState.AddModelError("", "Không thể đăng nhập!");
                }
            }
            return View();

        }




    }
}
