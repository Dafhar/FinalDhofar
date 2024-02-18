using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DhofarAppWeb.Model;
using DhofarAppWeb.Model.ViewModel;
using DhofarAppWeb.Data;
using Microsoft.EntityFrameworkCore;


namespace DhofarAppWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {


            var visitorCount = await _context.Visitors.CountAsync();



            var topUsers = await _context.Users
            // Order users by the sum of subjects created, comments made, and votes given
            .OrderByDescending(u => u.Subjects.Count + u.CommentSubjects.Count + u.UserVotes.Count) 
            .Take(5) // Take the top 5 users
            .ToListAsync();


            var compliants = await _context.Complaints.Where(c => c.IsAccepted == true).ToListAsync();

            var generalSubjects = await _context.GeneralSubjectsTypes.Include(gs => gs.Subjects).ThenInclude(s => s.User).ToListAsync();

            var subject = await _context.Subjects
                .Include(u=>u.User)
                .Include(f=>f.Files)
                .Include(s => s.CommentSubjects)
                .ThenInclude(cr => cr.CommentReplies)
                .OrderByDescending(s => s.LikeCounter + s.CommentSubjects.Count)
                .ThenBy(s => s.VisitorCounter)
                .FirstOrDefaultAsync();
            var suggestCount = 0;
      //      var suggests = await _context.Subjects
      //.Include(u => u.User)
      //.Include(st => st.SubjectTypeSubjects)
      //    .ThenInclude(sts => sts.SubjectType) // Include SubjectType in SubjectTypeSubjects
      //.Where(st => st.SubjectTypeSubjects.Select(s => s.SubjectType.Name_En == "suggest").FirstOrDefault()
      //)
      //.ToListAsync();

            //if (suggests == null)
            //{
            //    suggestCount = 0;
            //}
            //else
            //{
            //    suggestCount = suggests.Count();
            //}

            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Visitors = visitorCount,
                Users = topUsers,
                Complaints = compliants.Count(),
                GeneralSubjectsTypes = generalSubjects,
                Subjects = subject,
                SubjectSuggests = 0
            };

            ViewBag.HomeViewModel = homeViewModel;
            var generalSubjectsTypes = await _context.GeneralSubjectsTypes.ToListAsync();
            ViewBag.GeneralSubjectsTypes = generalSubjectsTypes;

            return View();
        }

        public IActionResult ContactUs()
        {
            Subject sub = new Subject();
            return View(sub);
        }

        public IActionResult Statstic()
        {
            return View();
        }

        public IActionResult Search_no_result()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string text)
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
