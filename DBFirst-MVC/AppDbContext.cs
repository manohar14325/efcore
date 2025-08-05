using Microsoft.EntityFrameworkCore;
using RestaurantApp.Models; // adjust namespace if needed

namespace RestaurantApp.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // keep generated mappings here (scaffold already placed them)
        }
    }
}
