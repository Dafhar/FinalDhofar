using DhofarAppApi.Data;
using DhofarAppApi.Dtos.Main;
using DhofarAppApi.Dtos.Subject;
using DhofarAppApi.Dtos.SubjectType;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.EntityFrameworkCore;

namespace DhofarAppApi.Services
{
    public class MainTopicServices : IMain
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenServices;

        public MainTopicServices(AppDbContext db, IHttpContextAccessor httpContextAccessor,JWTTokenServices jWTTokenServices)
        {

            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _jWTTokenServices = jWTTokenServices;

        }

        public async Task<SubjectForMainDTO> GetAll()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);

            var result = await _db.SubjectTypes
                .Include(subjectType => subjectType.SubjectTypeSubjects) 
                .ThenInclude(subject => subject.Subject)
                .Select(subjectType => new SubjectTypeDTO
                {
                    Title = user.SelectedLanguage == "en"? subjectType.Name_En: subjectType.Name_Ar,
                    SubjectDTO = new SubjectDTO
                    {
                        imeges = subjectType.SubjectTypeSubjects
                        .Select(subject => subject.Subject.User.ImageURL).ToList(), // Assuming Images is a collection of image paths in the Subject entity
                        count = subjectType.SubjectTypeSubjects.Count
                    }
                })
                .ToListAsync();

            return new SubjectForMainDTO
            {
                Title = user.SelectedLanguage == "en" ? "Main Topic" : "المحاور الرئيسية",
                subjectTypeDTOs = result
            };
        }
    }
}
