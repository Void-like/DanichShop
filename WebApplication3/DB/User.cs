using System;
using System.Collections.Generic;

namespace WebApplication3.DB;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Fname { get; set; } = null!;

    public string Sname { get; set; } = null!;

    public string Telephone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public decimal Balance { get; set; }

    public bool Ban { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<Korzina> Korzinas { get; set; } = new List<Korzina>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
