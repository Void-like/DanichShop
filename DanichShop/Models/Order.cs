using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanichShop.Models
{
    public class Order
    {
        public int IdUser { get; set; }
        public int ItemId { get; set; }
        public int Count {  get; set; }
        public decimal Price { get; set; }
    }
}
