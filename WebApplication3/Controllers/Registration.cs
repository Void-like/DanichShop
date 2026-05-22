using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using WebApplication3.DB;
using WebApplication3.Models.DTO;
using WebApplication3.Models.DTO.Auth;

namespace WebApplication3.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class Registration : ControllerBase
    {
        private readonly DanichshopContext _context;

        public Registration(DanichshopContext context)
        {
            _context = context;
        }
       
        [HttpPost("registration")]
        public async Task<IActionResult> Register( Register request)
        {
            if (request.Password != request.RPassword)
            {
                return new UnauthorizedObjectResult(new { message = "Повторите правильно пароль" });
            }
            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.FirstName) ||string.IsNullOrWhiteSpace(request.LastName))
            {
                return new UnauthorizedObjectResult(new { message = "Все поля заполняем" });
            }
            if (!request.Telephone.StartsWith("+7") && !request.Telephone.StartsWith("8") && request.Telephone.Length>11 && request.Telephone.Length < 10)
            {
                return new UnauthorizedObjectResult(new { message = "Такого номера телефона нету" });
            }
            if (!request.Email.Contains("@"))
            {
                return new UnauthorizedObjectResult(new { message = " Такой почты нет" });;
            }
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Login);

            if (existingUser != null)
            {
                return new UnauthorizedObjectResult(new { message = "Такой пользователь уже есть" });
            }
            else
            {
                var newUser = new User
                {
                    Fname = request.FirstName,
                    Sname = request.LastName,
                    Login = request.Login,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = "User",
                    Ban = false,
                    Telephone = request.Telephone,
                    Email = request.Email,
                    Balance = 0
                };
                await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "Регистрация успешна", user = newUser });

                
                
            }
           
           

       
          
        }
        

     
    }

}

