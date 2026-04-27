using System;
using System.Collections.Generic;

namespace WebApplication3.DB;

public partial class Korzina
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public int Count { get; set; }

    public virtual Item Item { get; set; }

    public virtual User User { get; set; }
}