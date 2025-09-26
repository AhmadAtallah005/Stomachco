using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stomachco.Models;

namespace Stomachco.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult view1()
        {
            return View();
        }
        [HttpPost]
        public IActionResult view1(RestFood re)
        {
            return RedirectToAction("Order");
        }
        [Authorize]
        public IActionResult view2()
        {
            return View();
        }
        [HttpPost]
        public IActionResult view2(CoffeeDrinks coffee)
        {
            return RedirectToAction("Order");
        }
        [Authorize]
        public IActionResult view3()
        {
            return View();
        }
        [HttpPost]
        public IActionResult view3(SMFood sm)
        {
            return RedirectToAction("Order");
        }
        [Authorize]

        public IActionResult Order() 
        {

            return View();
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
