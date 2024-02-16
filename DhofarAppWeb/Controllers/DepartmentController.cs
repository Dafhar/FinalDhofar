using DhofarAppWeb.Data;
using DhofarAppWeb.Dtos.DepartmentType;
using DhofarAppWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DhofarAppWeb.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _db;

        public DepartmentController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var departmentTypes = await _db.DepartmentTypes.ToListAsync();
            return View(departmentTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentTypeDTO departmentType)
        {
            if (ModelState.IsValid)
            {
                var newDepartmentType = new DepartmentType
                {
                    Name_En = departmentType.Name_En,
                    Name_Ar = departmentType.Name_Ar
                };

                _db.DepartmentTypes.Add(newDepartmentType);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departmentType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentType = await _db.DepartmentTypes.FindAsync(id);
            if (departmentType == null)
            {
                return NotFound();
            }
            return View(departmentType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentTypeDTO departmentType)
        {
          var department= await _db.DepartmentTypes.FindAsync(id);
            if (department == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                department.Name_Ar = departmentType.Name_Ar;

                 department.Name_En = departmentType.Name_En;
                  _db.Entry(department).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departmentType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentType = await _db.DepartmentTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departmentType == null)
            {
                return NotFound();
            }

            return View(departmentType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departmentType = await _db.DepartmentTypes.FindAsync(id);
            _db.DepartmentTypes.Remove(departmentType);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
    }
}
