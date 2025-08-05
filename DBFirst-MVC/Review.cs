using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RestaurantApp.Models;

public partial class Review
{
    [Key]
    public int Id { get; set; }

    public int RestaurantId { get; set; }

    public int? CustomerId { get; set; }

    public int Rating { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Reviews")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("RestaurantId")]
    [InverseProperty("Reviews")]
    public virtual Restaurant Restaurant { get; set; } = null!;
}
