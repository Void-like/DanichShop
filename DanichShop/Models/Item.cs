using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanichShop.Models
{
    public class Item
    {
        public int Id { get; set; }

        public string Title { get; set; } 

        public string Description { get; set; } 

        public decimal Cost { get; set; }

        public byte[] Picture { get; set; }

        public int Count { get; set; }
    }
}
