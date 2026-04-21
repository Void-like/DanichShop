using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication3.DB;
using WebApplication3.Models.DTO.Auth;

namespace WebApplication3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ItemControllers : ControllerBase
    {
        private readonly DanichshopContext _context;

        public ItemControllers(DanichshopContext context)
        {
            _context = context;
        }
 
        [HttpPost("createitem")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] ItemCreate itemCreate)
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
            try
            {
                await _context.Items.AddAsync(newItem);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Товар добавлен", item = newItem });

            }
            catch (Exception ex)
            {
                return new UnauthorizedObjectResult(new { message = $"Ошибка при сохранении: {ex.Message}" });

            }
        }
    }
}
