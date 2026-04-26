using System;
using System.Collections.Generic;

namespace WebApplication3.DB;

public partial class Item
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Cost { get; set; }

    public byte[] Picture { get; set; } = null!;

    public int Count { get; set; }

    public virtual ICollection<Korzina> Korzinas { get; set; } = new List<Korzina>();
}
