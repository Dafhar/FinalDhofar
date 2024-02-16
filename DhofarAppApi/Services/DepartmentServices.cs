using DhofarAppApi.Data;
using DhofarAppApi.Dtos.DepartmentType;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DhofarAppApi.Services
{
    public class DepartmentServices : IDepartmentType
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenServices;

        public DepartmentServices(AppDbContext db, IHttpContextAccessor httpContextAccessor, JWTTokenServices jWTTokenServices)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _jWTTokenServices=jWTTokenServices;
        }

       

        public async Task<List<GetDepartmentType>> GetAllDepartmentTypes()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);
            // Retrieve department names based on the user's language preference
            var departmentTypes = await _db.DepartmentTypes.ToListAsync();
            return departmentTypes.Select(dt => new GetDepartmentType
            {
                Name = user.SelectedLanguage == "en" ? dt.Name_En : dt.Name_Ar,
            }).ToList();
        }


        public async Task<DepartmentTypeDTO> CreateDepartmentType(DepartmentTypeDTO departmentType)
        {
            var newDepartmentType = new DepartmentType
            {
                Name_En = departmentType.Name_En,
                Name_Ar = departmentType.Name_Ar
            };

            await _db.DepartmentTypes.AddAsync(newDepartmentType);
            await _db.SaveChangesAsync();
            return departmentType;
        }

        public async Task<GetDepartmentType> GetDepartmentTypeById(int id)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);
            var departmentType = await _db.DepartmentTypes.FindAsync(id);
            return new GetDepartmentType
            {
                Name = user.SelectedLanguage == "en" ? departmentType.Name_En : departmentType.Name_Ar,
            };
        }

        public async Task<DepartmentTypeDTO> UpdateDepartmentType(int id, DepartmentTypeDTO updatedDepartmentType)
        {
            var departmentType = await _db.DepartmentTypes.FindAsync(id);
            if (departmentType == null)
            {
                return null; // DepartmentType not found
            }

            departmentType.Name_En = updatedDepartmentType.Name_En;
            departmentType.Name_Ar = updatedDepartmentType.Name_Ar;

            await _db.SaveChangesAsync();
            return updatedDepartmentType;
        }

        public async Task<bool> DeleteDepartmentType(int id)
        {
            var departmentType = await _db.DepartmentTypes.FindAsync(id);
            if (departmentType == null)
            {
                return false; // DepartmentType not found
            }

            _db.DepartmentTypes.Remove(departmentType);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
