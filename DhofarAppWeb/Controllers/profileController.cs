using DhofarAppWeb.Data;
using DhofarAppWeb.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DhofarAppWeb.Controllers
{
    public class profileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public profileController(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager; 
            _context = context;
        }

        public async Task<IActionResult> IndexProfile()
        {
            var getUser = await _userManager.GetUserAsync(this.User);
            if (getUser == null)
            {
                return View(null);
            }
            else
            {
                var user = await _context.Users
               .Include(s => s.Subjects)
               .Include(c => c.CommentSubjects)
               .Include(cr => cr.CommentReplies)
               .Include(co => co.Complaints)
               .FirstOrDefaultAsync(u => u.Id == getUser.Id);

                return View(user);
            }
           
        }






        public IActionResult EditProfile()
        {
            return View();
        }
    }
}
