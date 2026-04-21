using Avalonia.Media.Imaging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
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

        public Bitmap PictureImage
        {
            get
            {
                if (Picture == null || Picture.Length == 0)
                    return null;

                using var ms = new MemoryStream(Picture);
                return new Bitmap(ms);
            }
        }



        public int Count { get; set; }
    }
}
