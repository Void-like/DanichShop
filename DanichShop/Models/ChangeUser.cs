using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanichShop.Models
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
