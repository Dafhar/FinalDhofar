using DhofarAppWeb.Data;
using DhofarAppWeb.Dtos.User;
using DhofarAppWeb.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Resources;
using System.Security.Claims;

namespace DhofarAppWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AppDbContext _context;
        private string SecretKey = "$2y$10$.vBG561wmRjrwAVXD98HNOgsNpDczlqm3Jq9QnEd1rVAGv3Fykk1a";


        public UserController(SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(RegisterDTO registerUser, ModelStateDictionary modelState, ClaimsPrincipal principal)
        {
            if (ModelState.IsValid)
            {
                var lang = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString().Split(',')[0].Trim();
                if (string.IsNullOrEmpty(lang))
                {
                    // Set default language if Accept-Language header is not provided or empty
                    lang = "ar";
                }

                var lastIdentityNumber = await _context.IdentityNumbers.FirstOrDefaultAsync();
                int generateIdentityId = (lastIdentityNumber != null) ? lastIdentityNumber.Value : 1;

                var NewUser = new User
                {
                    UserName = registerUser.UserName,
                    FullName = registerUser.Fullname,
                    Email = registerUser.Email,
                    PhoneNumber = registerUser.PhoneNumber,
                    IdentityNumber = generateIdentityId++,
                    JoinedDate = DateTime.UtcNow,
                    SelectedLanguage = lang
                };

                var phoneNumberExist = await _userManager.Users.AnyAsync(u => u.PhoneNumber == NewUser.PhoneNumber);
                if (phoneNumberExist)
                {
                    modelState.AddModelError(nameof(registerUser.PhoneNumber), "PhoneNumber must be unique.");
                    return View();
                }

                var result = await _userManager.CreateAsync(NewUser, registerUser.Password);

                if (result.Succeeded)
                {
                    IList<string> role = new List<string>() { "User" };
                    await _userManager.AddToRolesAsync(NewUser, role);

                    if (lastIdentityNumber != null)
                    {
                        lastIdentityNumber.Value = generateIdentityId;
                    }
                    else
                    {
                        await _context.IdentityNumbers.AddAsync(new IdentityNumber { Value = generateIdentityId });
                    }
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Home", "ControllerName");

                }
                else
                {
                    return PartialView("_RegisterPartial", registerUser);


                }

            }
                         return PartialView("_RegisterPartial", registerUser);
            
        }

    }

}