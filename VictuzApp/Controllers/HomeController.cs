using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VictuzApp.Models;
using VictuzApp.Services;

namespace VictuzApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public BestActivityService _bestactivityService;


        public HomeController(ILogger<HomeController> logger, BestActivityService bestactivityService)
        {
            _logger = logger;
            _bestactivityService = bestactivityService;
        }

        public async Task<IActionResult> Index()
        {
            var discounts = await _bestactivityService.GetDiscountsAsync();
            return View(discounts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
