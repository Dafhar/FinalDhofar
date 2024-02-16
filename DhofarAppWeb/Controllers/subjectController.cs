using Microsoft.AspNetCore.Mvc;

namespace DhofarAppWeb.Controllers
{
    public class subjectController : Controller
    {

// the lsit of subject
        public IActionResult IndexSubject()
        {
            return View();
        }

        public IActionResult AddSubject()
        {
            return View();
        }

        public IActionResult ViewSubject()
        {
            return View();
        }

        public IActionResult Mytopic()
        {
            return View();
        }

    }
}
