using Microsoft.AspNetCore.Mvc;

namespace DhofarAppWeb.Controllers
{
    public class profileController : Controller
    {
        public IActionResult IndexProfile()
        {
            return View();
        }





        [HttpPost]
        public IActionResult EditProfile()
        {
            return RedirectToAction("IndexProfile", "profile");
        }
    }
}
