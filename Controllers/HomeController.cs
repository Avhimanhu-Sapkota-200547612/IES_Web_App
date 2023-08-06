
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IES_WebAuth_Project.Models;

namespace IES_WebAuth_Project.Controllers
{
    // HomeController handles requests related to the home page and error views.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Constructor to initialize the HomeController with a logger.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /Home/Index
        // Displays the default home page view.
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy
        // Displays the Privacy view.
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/Error
        // Displays the Error view with details of the current error.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Creates an ErrorViewModel with the current request ID or the trace identifier.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
