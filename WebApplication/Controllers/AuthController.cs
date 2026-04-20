using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication.Models.DTO.Auth;


//namespace WebApplication1.Controllers
//{
//    [Authorize]
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : Controller
//    {
//        private readonly _1135InventorySystemContext _context;

//        public AuthController(_1135InventorySystemContext context)
//        {
//            _context = context;
//        }

//    [HttpPost]
//    [Route("login")]
//    public async Task<ActionResult> Login(LoginData loginData)
//    {
//        try
//        {
//            var user = await _context.Users.FirstOrDefaultAsync(u =>
//                loginData.Login == u.Username && u.IsActive == true);
        
//            if (user == null)
//                return Unauthorized(new { message = "Такого пользователя не существует" });
        
//            if (!VerifyPassword(loginData.Password, user.PasswordHash))
//                return Unauthorized(new { message = "Неверный пароль" });

//            var claims = new List<Claim> {
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
//            };
        
//            var jwt = new JwtSecurityToken(
//                issuer: AuthOptions.ISSUER,
//                audience: AuthOptions.AUDIENCE,
//                claims: claims,
//                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
//                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
//            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
        
//            user.LastLogin = DateTime.Now;
//            await _context.SaveChangesAsync();
        

//            return Ok(token);
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine($"Ошибка сервера при авторизации: {e.Message}");
//            return StatusCode(500, new { message = "Ошибка сервера" });
//        }
//    }

//            private bool VerifyPassword(string inputPassword, string storedHash)
//        {
//            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
//        }       
//    }   
//}


