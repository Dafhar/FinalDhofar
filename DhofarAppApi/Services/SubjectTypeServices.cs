using DhofarAppApi.Data;
using DhofarAppApi.Dtos.Subject;
using DhofarAppApi.Dtos.SubjectType;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using DhofarAppApi.Services;

namespace DhofarAppWeb.Services
{
    public class SubjectTypeServices : ISubjectType
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenServices;

        public SubjectTypeServices(AppDbContext db, IHttpContextAccessor httpContextAccessor, JWTTokenServices jWTTokenServices)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor; 
            _jWTTokenServices= jWTTokenServices;
        }

        public async Task<List<GetSubjectTypeDTO>> GetAllSubjectTypes()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
           var user= await _db.Users.FindAsync(userId);
            var subjectTypes = await _db.SubjectTypes
                .Select(st => new GetSubjectTypeDTO
                {
                    Name = user.SelectedLanguage== "en" ? st.Name_En : st.Name_Ar,
                    TitleValue = st.TitleValue,
                    Subjects = st.Subjects.Select(s => new GetSubjectDTO
                    {
                        Title= s.Title,
                    }).ToList()
                })
                .ToListAsync();

            return subjectTypes;
        }

        public async Task<GetSubjectTypeDTO> GetSubjectTypeById(int id)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);
            var subjectType = await _db.SubjectTypes
                .Where(st => st.Id == id)
                .Select(st => new GetSubjectTypeDTO
                {
                    Name = user.SelectedLanguage == "en" ? st.Name_En : st.Name_Ar,
                    TitleValue=st.TitleValue,
                    
                    Subjects = st.Subjects.Select(s => new GetSubjectDTO
                    {
                        Title = s.Title,
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return subjectType;
        }

        public async Task<SubjectType> CreateSubjectType(PostSubjectTypeDTO postSubjectTypeDto)
        {
            var subjectType = new SubjectType
            {
                Name_En = postSubjectTypeDto.Name_En,
                Name_Ar=postSubjectTypeDto.Name_Ar,
                TitleValue=postSubjectTypeDto.TitleValue,
            };

            await _db.SubjectTypes.AddAsync(subjectType);
            await _db.SaveChangesAsync();

            return subjectType;
        }

        public async Task<SubjectType> UpdateSubjectType(int id, PostSubjectTypeDTO postSubjectTypeDto)
        {
            var subjectType = await _db.SubjectTypes.FindAsync(id);

            if (subjectType == null)
                return null;

            subjectType.Name_Ar = postSubjectTypeDto.Name_Ar;
            subjectType.Name_En = postSubjectTypeDto.Name_En;
            subjectType.TitleValue = postSubjectTypeDto.TitleValue;

            _db.Entry(subjectType).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return subjectType;
        }

        public async Task<bool> DeleteSubjectType(int id)
        {
            var subjectType = await _db.SubjectTypes.FindAsync(id);

            if (subjectType == null)
                return false;

            _db.SubjectTypes.Remove(subjectType);
            await _db.SaveChangesAsync();

            return true;
        }


    }
}
