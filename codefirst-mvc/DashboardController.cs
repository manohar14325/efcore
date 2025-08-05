using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementApp.Data;
using StudentManagementApp.Models;
using System.Net.Http.Json;

namespace StudentManagementApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpFactory;

        public DashboardController(AppDbContext context, IHttpClientFactory httpFactory)
        {
            _context = context;
            _httpFactory = httpFactory;
        }

        public async Task<IActionResult> Index()
        {
            // 1) total students
            var totalStudents = await _context.Students.CountAsync();

            // 2) top students by marks (take 6)
            var topStudents = await _context.Students
                                .OrderByDescending(s => s.Marks)
                                .Take(6)
                                .ToListAsync();

            // 3) upcoming birthdays: same month and day >= today
            var today = DateTime.Now;
            var upcomingBirthdays = await _context.Students
                .Where(s => s.DateOfBirth.HasValue &&
                       (s.DateOfBirth.Value.Month == today.Month &&
                        s.DateOfBirth.Value.Day >= today.Day))
                .OrderBy(s => s.DateOfBirth.Value.Day)
                .Take(6)
                .ToListAsync();

            // 4) Quote of the day using public API (quotable.io) - best-effort
            string quoteText = "Stay motivated!";
            try
            {
                var c = _httpFactory.CreateClient();
                c.Timeout = TimeSpan.FromSeconds(5);
                var q = await c.GetFromJsonAsync<Quotable>("https://api.quotable.io/random");
                if (q != null && !string.IsNullOrWhiteSpace(q.content))
                    quoteText = $"{q.content} — {q.author}";
            }
            catch
            {
                // ignore API failures — fallback quote remains
            }

            // build viewmodel and return
            var vm = new DashboardViewModel
            {
                TotalStudents = totalStudents,
                TopStudents = topStudents,
                UpcomingBirthdays = upcomingBirthdays,
                Quote = quoteText
            };

            return View(vm);
        }

        private class Quotable
        {
            public string content { get; set; }
            public string author { get; set; }
        }
    }

    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public List<Student> TopStudents { get; set; } = new();
        public List<Student> UpcomingBirthdays { get; set; } = new();
        public string Quote { get; set; } = string.Empty;
    }
}
