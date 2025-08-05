using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;

namespace RestaurantApp.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly AppDbContext _context;
        public RestaurantsController(AppDbContext context) => _context = context;

        // GET: /Restaurants
        public async Task<IActionResult> Index()
        {
            var list = await _context.Restaurants.ToListAsync();
            return View(list);
        }

        // GET: /Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == id);
            if (restaurant == null) return NotFound();

            // menu items for this restaurant
            var menu = await _context.MenuItems
                .Where(m => m.RestaurantId == restaurant.Id)
                .ToListAsync();

            ViewBag.MenuItems = menu;
            return View(restaurant);
        }
    }
}
