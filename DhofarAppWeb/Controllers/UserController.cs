using DhofarAppWeb.Data;
using DhofarAppWeb.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Resources;
using System.Security.Claims;
using DhofarAppWeb.Model.Interface;
using DhofarAppWeb.Dtos.User;

namespace DhofarAppWeb.Controllers
{
    public class UserController : Controller
    {

        private readonly IUser _context;

        public UserController(IUser context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var user = await _context.Register(registerDTO, this.ModelState);
            if (ModelState.IsValid)
            {
                // If the registration was successful, return a JSON response indicating success
                return Json(new { success = true });
            }
            else
            {

                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage);
                return Json(new { success = false, errors = errors });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputDTO loginDto)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage);
                return Json(new { success = false, errors = errors });
            }

            var user = await _context.Login(loginDto, this.ModelState);
            if (user != null)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
            }

        }

}