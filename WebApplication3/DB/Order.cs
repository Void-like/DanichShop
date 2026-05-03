using System;
using System.Collections.Generic;

namespace WebApplication3.DB;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public int Count { get; set; }

    public decimal Price { get; set; }

    public DateTime? OrderDate { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
