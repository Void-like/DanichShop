namespace WebApplication3.Models.DTO.Auth
{
    public class ChangeUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
