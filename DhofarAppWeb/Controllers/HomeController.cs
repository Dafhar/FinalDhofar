using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DhofarAppWeb.Model;


namespace DhofarAppWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()

        {

            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Statstic()
        {
            return View();
        }

        public IActionResult Search_no_result()
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
