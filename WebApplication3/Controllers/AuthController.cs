using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApplication3.Auth;
using WebApplication3.DB;
using WebApplication3.Models.DTO.Auth;


namespace WebApplication3.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly DanichshopContext _context;

        public LoginController(DanichshopContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginData loginData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.Login == loginData.Login);
            if (user == null)
                return new NotFoundResult();
            if (!VerifyPassword(loginData.Password, user.Password))
                return new UnauthorizedObjectResult(new { message = "Неверный пароль" });
            if(user.Ban == true)
            {
                return new UnauthorizedObjectResult(new { message = "ТЫ ЗАБАНЕН" });
            }



            var claims = new List<Claim> {
          
              new Claim(ClaimValueTypes.Integer32, user.Id.ToString()),
              new Claim(ClaimTypes.Name, user.Login),
              new Claim(ClaimTypes.GivenName, user.Fname),
              new Claim(ClaimTypes.Surname, user.Sname),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.MobilePhone, user.Telephone),
              new Claim(ClaimTypes.Role, user.Role),
              new Claim("balance", user.Balance.ToString())
        };
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,             
                claims: claims,             
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new OkObjectResult(token);
        }

        [HttpGet]
        [Route("changePassword")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(string oldPassword, string newPassword,int userid)
        {
            try
            {
                
                if (userid == null)
                    return new UnauthorizedObjectResult(new { message = "Вы не зарегистрированы" });

              
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid);

                if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
                    return BadRequest(new { message = "Введите старый пароль" });

                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка сервера при смене пароля: {e.Message}");
                return StatusCode(500, new { message = "Ошибка сервера" });
            }

        }
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }
    }
}


