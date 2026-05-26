using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.DB;
using WebApplication3.Models.DTO.Auth;
namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
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

                return Ok(new { message = $"Количество товара  увеличено" });
            }


            var cartItem = new Korzina
            {
                UserId = dto.UserID,
                ItemId = dto.ItemsID,
                Count = dto.Count
            };

            _context.Korzinas.Add(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Товар  добавлен в корзину" });
        }
        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var cartItems = await _context.Korzinas.Where(x => x.UserId == userId).ToListAsync();
            if (cartItems.Count == 0)
            {
                return Ok(new { message = "Корзина уже пуста" });
            }
            _context.Korzinas.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Ok(new{message = "корзина очищена"});
        }

        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetUserCart(int userId)
        {

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Пользователь не найден" });
            }


            var cartItems = await _context.Korzinas.Where(c => c.UserId == userId).ToListAsync();


            if (cartItems == null || cartItems.Count == 0)
            {
                return Ok(new List<Item>());
            }


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
        [HttpPost("buyitem")]
        [Authorize]
        public async Task<IActionResult> Buy([FromBody] KorzinaDTO dto)
        {
            var user = await _context.Users.FindAsync(dto.UserID);
            if (user == null)
            {
                return BadRequest(new { message = $"Пользователь с ID {dto.UserID} не найден" });
            }
            var item = await _context.Items.FindAsync(dto.ItemsID);
            if (item == null)
            {
                return BadRequest(new { message = $"Товар с ID {dto.ItemsID} не найден" });
            }
            var existingItem = await _context.Korzinas.FirstOrDefaultAsync(x => x.UserId == dto.UserID && x.ItemId == dto.ItemsID);
           
            decimal price = item.Cost * existingItem.Count;
            if (price > user.Balance) 
            {
                return BadRequest(new { message = "Недостаточно средств" });
            }
            else if (item.Count < existingItem.Count)
            {
                return BadRequest(new { message = "Недостаточно товаров" });
            }
            else
    {
        var newOrder = new Order
        {
            ItemId = dto.ItemsID,
            UserId = dto.UserID,
            Count = existingItem.Count,
            Price = price,
            OrderDate = DateTime.Now,
        };

        item.Count = item.Count - existingItem.Count;
        user.Balance = user.Balance - price;
        await _context.Orders.AddAsync(newOrder);
        _context.Korzinas.Remove(existingItem);
        await _context.SaveChangesAsync();
        return BadRequest(new { message = "Покупка успешна" });

    }

        }
        [HttpPost("del")]
        public async Task<IActionResult> DelToCart([FromBody] KorzinaDTO dto)
        {

            var user = await _context.Users.FindAsync(dto.UserID);
            if (user == null)
            {
                return BadRequest(new { message = $"Пользователь с ID {dto.UserID} не найден" });
            }
            var item = await _context.Items.FindAsync(dto.ItemsID);
            if (item == null)
            {
                return BadRequest(new { message = $"Товар с ID {dto.ItemsID} не найден" });
            }
            var existingItem = await _context.Korzinas.FirstOrDefaultAsync(x => x.UserId == dto.UserID && x.ItemId == dto.ItemsID);

            if (existingItem != null)
            {
                if (existingItem.Count <= 1)
                {
                   
                    _context.Korzinas.Remove(existingItem);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = $"Товар удален" });
                }
                else
                {
                    existingItem.Count = existingItem.Count - 1;
                    await _context.SaveChangesAsync();


                    var cartItem = new Korzina
                    {
                        UserId = dto.UserID,
                        ItemId = dto.ItemsID,
                        Count = dto.Count
                    };

                    
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Товар убавлен" });
                }
            }
            else
            {
                return BadRequest(new { message = "какая-то ошибка" });

            }
        }
    }
}
