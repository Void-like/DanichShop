namespace WebApplication3.Models.DTO.Auth
{
    public class Register
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string RPassword { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string Telephone { get; set; } = null!;
        public string Email { get; set; } = null!;

    }
}
