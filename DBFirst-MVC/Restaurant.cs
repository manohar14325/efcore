using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RestaurantApp.Models;

public partial class Restaurant
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(250)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    public string? Description { get; set; }

    [StringLength(250)]
    public string? ImageUrl { get; set; }

    [InverseProperty("Restaurant")]
    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    [InverseProperty("Restaurant")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
