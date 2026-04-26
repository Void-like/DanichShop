using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.DB;
using WebApplication3.Models.DTO.Auth;
namespace WebApplication3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class KorzinaController : ControllerBase
    {



        private readonly DanichshopContext _context;

        public KorzinaController(DanichshopContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] KorzinaDTO dto)
        {
            
            var user = await _context.Users.FindAsync(dto.UserID);
            if (user == null)
                return BadRequest(new { message = $"Пользователь с ID {dto.UserID} не найден" });

            var item = await _context.Items.FindAsync(dto.ItemsID);
            if (item == null)
                return BadRequest(new { message = $"Товар с ID {dto.ItemsID} не найден" });

            var existingItem = await _context.Korzinas.FirstOrDefaultAsync(x => x.UserId == dto.UserID && x.ItemId == dto.ItemsID);

            if (existingItem != null)
            {
           
                existingItem.Count = existingItem.Count + 1;
                await _context.SaveChangesAsync();

                return Ok(new {message = $"Количество товара  увеличено"});
            }

           
            var cartItem = new Korzina
            {
                UserId = dto.UserID,
                ItemId = dto.ItemsID,
                Count = dto.Count
            };

            _context.Korzinas.Add(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new  {message = $"Товар  добавлен в корзину"});
        }

       
        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetUserCart(int userId)
        {
            // Проверяем, есть ли пользователь
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Пользователь не найден" });
            }

            // Получаем все записи корзины этого пользователя
            var cartItems = await _context.Korzinas
                .Where(c => c.UserId == userId)
                .ToListAsync();

            // Если корзина пуста
            if (cartItems == null || cartItems.Count == 0)
            {
                return Ok(new List<Item>());
            }

            // Собираем результат с информацией о товарах
            var result = new List<Item>();

            foreach (var cart in cartItems)
            {
               
                var item = await _context.Items.FindAsync(cart.ItemId);
                var existingItem = await _context.Korzinas.FirstOrDefaultAsync(x => x.UserId == userId && x.ItemId == cart.ItemId);
                if (item != null)
                {
                    result.Add(new Item
                    {
                        Id = cart.ItemId,
                        Title = item.Title,
                        Description = item.Description,
                        Picture = item.Picture,
                        Cost = item.Cost,
                        Count = existingItem.Count



                    });
                }
            }

            return Ok(result);
        }
    }
}
