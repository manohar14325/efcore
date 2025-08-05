using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RestaurantApp.Models;

public partial class MenuItem
{
    [Key]
    public int Id { get; set; }

    public int RestaurantId { get; set; }

    public int? CategoryId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public bool IsVegetarian { get; set; }

    [StringLength(250)]
    public string? ImageUrl { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("MenuItems")]
    public virtual Category? Category { get; set; }

    [InverseProperty("MenuItem")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("RestaurantId")]
    [InverseProperty("MenuItems")]
    public virtual Restaurant Restaurant { get; set; } = null!;
}
