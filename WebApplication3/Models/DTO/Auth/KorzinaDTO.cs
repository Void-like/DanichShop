using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Models.DTO.Auth
{
   
    public class KorzinaDTO
    {
        public int UserID { get; set; }
        public int ItemsID { get; set; }
        public int Count { get; set; }
    }
}
