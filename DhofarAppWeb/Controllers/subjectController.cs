using DhofarAppWeb.Data;
using DhofarAppWeb.Dtos.Subject;
using DhofarAppWeb.Model;
using DhofarAppWeb.Model.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DhofarAppWeb.Controllers
{
    public class subjectController : Controller
    {

        private readonly AppDbContext _context;

        public subjectController(AppDbContext context)
        {
            _context = context;
        }
        // the lsit of subject
        public async Task<IActionResult> IndexSubject(int generalSubjectId)
        {

            var subjects = await _context.Subjects
        .Where(s => s.GeneralSubjectsTypesId == generalSubjectId)
        .Include(sf => sf.Files)
        .Include(u => u.User)
        .Include(gs => gs.GeneralSubjectsTypes)
        .Include(cs => cs.CommentSubjects)
        .ThenInclude(csu => csu.User)
        .Include(st => st.SubjectTypeSubjects)
        .ThenInclude(st => st.SubjectType)
        .ToListAsync();

            var generalSubject = await _context.GeneralSubjectsTypes.FirstOrDefaultAsync(gs => gs.Id == generalSubjectId);

            SubjectGeneralSubject subjectGeneralSubject = new SubjectGeneralSubject()
            {
                Subjects = subjects,
                GeneralSubjectsType = generalSubject
            };

            return View(subjectGeneralSubject);


        }

        public async Task<IActionResult> AddSubject(int generalSubjectId)
        {
            var subjectTypes = await _context.SubjectTypes.ToListAsync();

            ViewBag.SubjectTypes = new SelectList(subjectTypes, "Id", "Name_En");
            var subject = new PostSubjectDTO()
            {
                GeneralSubjectsTypesId = generalSubjectId
            };
            return View(subject);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(PostSubjectDTO subject)
        {
            if (ModelState.IsValid)
            {
                // Save the subject to the database
                var newSubject = new Subject()
                {
                    GeneralSubjectsTypesId = subject.GeneralSubjectsTypesId,
                    PrimarySubject = subject.PrimarySubject,
                    Title = subject.Title,
                    Description = subject.Description,
                    UserId = subject.UserId,
                    CreatedTime = DateTime.UtcNow,
                };

                await _context.Subjects.AddAsync(newSubject);
                await _context.SaveChangesAsync();

                SubjectTypeSubject subjectTypeSubject = new SubjectTypeSubject()
                {
                    SubjectId = newSubject.Id,
                    SubjectTypeId = subject.SubjectTypeId,
                };

                await _context.SubjectTypeSubjects.AddAsync(subjectTypeSubject);
                await _context.SaveChangesAsync();

                // Redirect to IndexSubject action with the generalSubjectId parameter
                return RedirectToAction("IndexSubject", new { generalSubjectId = subject.GeneralSubjectsTypesId });
            }

            // If model state is not valid, return to the form with validation errors
            return View(subject);
        }


        // If model state is not valid, return to the form with validation errors


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
