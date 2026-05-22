using Humanizer;
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
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly DanichshopContext _context;

        public LoginController(DanichshopContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginData loginData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.Login == loginData.Login);
            if (user == null) return new NotFoundResult();
            if (!VerifyPassword(loginData.Password, user.Password)) return new UnauthorizedObjectResult(new { message = "Неверный пароль" });
            if(user.Ban == true) return new UnauthorizedObjectResult(new { message = "ТЫ ЗАБАНЕН" });
           



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

        [HttpPost]
        [Route("changepassword")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassword changePassword)
        {
            try
            {
                
                if (changePassword.Id == null) return new UnauthorizedObjectResult(new { message = "Вы не зарегистрированы" });

              
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == changePassword.Id);

                if (!BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, user.Password)) return BadRequest(new { message = "Введите старый пароль" });

                user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка сервера при смене пароля: {e.Message}");
                return StatusCode(500, new { message = "Ошибка сервера" });
            }

        }

        [HttpPost]
        [Route("changeaccount")]
        public async Task<ActionResult> ChangeAccount([FromBody] ChangeUser changeUser)
        {
            var user = await _context.Users.FindAsync(changeUser.Id);

            if (user == null)
            {
                return NotFound(new { message = $"Пользователь не найден" });
            }

            user.Login = changeUser.Login;
            user.Fname = changeUser.FirstName;
            user.Sname = changeUser.LastName;
            user.Telephone = changeUser.Telephone;
            user.Email = changeUser.Email;

           
            await _context.SaveChangesAsync();

            return Ok(new{message ="Пользователь  успешно обновлён" });

        }

        [HttpPost]
        [Route("adminchangeuser")]
        [Authorize]
        public async Task<ActionResult> AdminChangeUser([FromBody] User changeUser)
        {
            var user = await _context.Users.FindAsync(changeUser.Id);

            if (user == null)
            {
                return NotFound(new { message = $"Пользователь не найден" });
            }




            user.Login = changeUser.Login;
            user.Fname = changeUser.Fname;
            user.Sname = changeUser.Sname;
            user.Telephone = changeUser.Telephone;
            user.Email = changeUser.Email;
            user.Ban = changeUser.Ban;
            user.Role = changeUser.Role;
            user.Balance = changeUser.Balance;

           await _context.SaveChangesAsync();

            return Ok(new { message = "Пользователь  успешно обновлён" });

        }
        [HttpPost]
        [Route("Addbalance")]
        [Authorize]
        public async Task<ActionResult> addbalance([FromBody] AddBalance addBalance)
        {
            var user = await _context.Users.FindAsync(addBalance.Id);

            if (user == null)
            {
                return NotFound(new { message = $"Пользователь не найден" });
            }




            user.Balance = user.Balance + addBalance.addbalance;


            await _context.SaveChangesAsync();

            return Ok(new { message = $"Баланс пополнен" });

        }
        [Authorize]
        [HttpGet("getuser")]
        public async Task<ActionResult<List<User>>> GetUser()
        {

            return await _context.Users.ToListAsync();
        }
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }
    }
}


