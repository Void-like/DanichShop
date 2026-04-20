using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanichShop.Models
{
    public partial class User
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int Id { get; set; }

        public decimal Balance { get; set; } = 0;

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;

        public bool Ban { get; set; } = false;
    }
}
