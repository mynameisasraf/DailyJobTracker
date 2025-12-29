using System.Diagnostics;
using DailyJobTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace DailyJobTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Redirect Home/Index to Dashboard in DailyJobsController
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard", "DailyJobs");
        }

        // Optional: remove Privacy if not needed, or repurpose
        public IActionResult Privacy()
        {
            return RedirectToAction("Index", "DailyJobs");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
