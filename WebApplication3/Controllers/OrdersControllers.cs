using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication3.DB;
using WebApplication3.Models.DTO.Auth;

namespace WebApplication3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersControllers : ControllerBase
    {
        private readonly DanichshopContext _context;

        public OrdersControllers(DanichshopContext context)
        {
            _context = context;
        }

        [HttpPost("BuyItem")]
        [Authorize]
        public async Task<IActionResult> Buy([FromBody] ItemCreate itemCreate)
        {
            var newOrder = new Order
            {



                OrderDate = DateTime.Now,
            };
            try
            {
                await _context.Orders.AddAsync(newOrder);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Товар куплен" });

            }
            catch (Exception ex)
            {
                return new UnauthorizedObjectResult(new { message = $"Ошибка при покупке: {ex.Message}" });

            }


        }
    }
}
