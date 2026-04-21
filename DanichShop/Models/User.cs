using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanichShop.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Fname { get; set; } 

        public string Sname { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        public decimal Balance { get; set; }

        public bool Ban { get; set; }

        public string Role { get; set; }
    }
}
