using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication3.DB;
using WebApplication3.Models.DTO.Auth;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemControllers : ControllerBase
    {
        private readonly DanichshopContext _context;

        public ItemControllers(DanichshopContext context)
        {
            _context = context;
        }
       
        [HttpPost("createitem")]
        [Authorize]
        public async Task<IActionResult> Create( ItemCreate itemCreate)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimValueTypes.Integer32);
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userIdClaim == null || userRole != "Admin")
            {
                return Forbid("Только админ может добавлять предметы");
            }

            if (string.IsNullOrWhiteSpace(itemCreate.Title) || string.IsNullOrWhiteSpace(itemCreate.Description))
            {
                return new UnauthorizedObjectResult(new { message = "Все поля заполняем" });
            }
            if (itemCreate.Picture == null)
            {
                return new UnauthorizedObjectResult(new { message = "Нету картинки" });
            }

            var newItem = new Item
            {
               Title = itemCreate.Title,
               Description = itemCreate.Description,
               Cost = itemCreate.Cost,
               Picture = itemCreate.Picture,
               Count = itemCreate.Count,
            };
           
                await _context.Items.AddAsync(newItem);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Товар добавлен", item = newItem });

          

           
        }
        [HttpPost("changeitem")]
        [Authorize]
        public async Task<IActionResult> Change( ItemCreate itemCreate)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimValueTypes.Integer32);
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var Product = _context.Items.Find(itemCreate.Id);
            if (userIdClaim == null || userRole != "Admin")
            {
                return Forbid("Только админ может добавлять предметы");
            }
            if (itemCreate.Id == 0)   
            {
                return Forbid("Товара НЕТУ");
            }

            if (!string.IsNullOrWhiteSpace(itemCreate.Title) )
            {
                Product.Title = itemCreate.Title;
            }
            if (!string.IsNullOrWhiteSpace(itemCreate.Description))
            {
                Product.Description = itemCreate.Description;
            }          
                Product.Cost = itemCreate.Cost;        
            if (itemCreate.Picture != null)
            {
                Product.Picture = itemCreate.Picture;
            }
               Product.Count = itemCreate.Count;
         
            
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Товар изменен"});

        }
         


        
        [Authorize]
        [HttpDelete("Delete/{itemId}")]

        public async Task<IActionResult> Deleteitem(int itemId)
        {
            var cartItems = await _context.Items.Where(x => x.Id == itemId).ToListAsync();
            if (cartItems != null)
            {
                _context.Items.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Предмет удален" });
            }
            else
            {
                return new UnauthorizedObjectResult(new { message = "Ошибка при удалении" });
            }
        }
        [Authorize]
        [HttpGet("getitem")]
        public async Task<ActionResult<List<Item>>> GetItems()
        {

            return await _context.Items.ToListAsync();
        }
    }
}
