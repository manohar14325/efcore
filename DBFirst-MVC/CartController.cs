using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Helpers;
using RestaurantApp.ViewModels;

namespace RestaurantApp.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private const string SessionKey = "CartItems";

        public CartController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> AddToCart(int menuItemId, int quantity = 1)
        {
            var menu = await _context.MenuItems.FindAsync(menuItemId);
            if (menu == null) return NotFound();

            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(SessionKey) ?? new();

            var existing = cart.FirstOrDefault(c => c.MenuItemId == menuItemId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItemViewModel
                {
                    MenuItemId = menu.Id,
                    MenuItemName = menu.Name,
                    UnitPrice = menu.Price,
                    Quantity = quantity,
                    RestaurantId = menu.RestaurantId
                });
            }

            HttpContext.Session.SetObject(SessionKey, cart);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(SessionKey) ?? new();
            ViewBag.Total = cart.Sum(i => i.Total);
            return View(cart);
        }

        [HttpPost]
        public IActionResult Remove(int menuItemId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(SessionKey) ?? new();
            var item = cart.FirstOrDefault(c => c.MenuItemId == menuItemId);
            if (item != null) cart.Remove(item);
            HttpContext.Session.SetObject(SessionKey, cart);
            return RedirectToAction("Index");
        }

        // GET: Checkout page (show form)
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(SessionKey) ?? new();
            if (!cart.Any()) return RedirectToAction("Index");

            return View();
        }

        // POST: Checkout - create customer, order, orderitems, clear cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(RestaurantApp.ViewModels.CheckoutViewModel model)
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(SessionKey) ?? new();
            if (!cart.Any()) { ModelState.AddModelError("", "Cart is empty."); return View(model); }
            if (!ModelState.IsValid) return View(model);

            // create or find customer by email
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
            if (customer == null)
            {
                customer = new Models.Customer
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            // create order
            var order = new Models.Order
            {
                CustomerId = customer.Id,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = cart.Sum(i => i.Total)
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // create order items
            foreach (var ci in cart)
            {
                var oi = new Models.OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = ci.MenuItemId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice
                    // TotalPrice is computed column in DB
                };
                _context.OrderItems.Add(oi);
            }
            await _context.SaveChangesAsync();

            // clear cart
            HttpContext.Session.SetObject(SessionKey, new List<CartItemViewModel>());

            return RedirectToAction("Details", "Orders", new { id = order.Id });
        }
    }
}
