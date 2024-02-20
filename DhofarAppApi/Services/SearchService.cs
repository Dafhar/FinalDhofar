using DhofarAppApi.Data;
using DhofarAppApi.Dtos.Search;
using DhofarAppApi.Dtos.SubjectFiles;
using DhofarAppApi.InterFaces;
using Microsoft.EntityFrameworkCore;

namespace DhofarAppApi.Services
{
    public class SearchService : ISearch
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenServices;

        public SearchService(AppDbContext context , IHttpContextAccessor httpContextAccessor, JWTTokenServices jWTTokenServices)
        {
            _context = context; 
            _httpContextAccessor = httpContextAccessor;
            _jWTTokenServices = jWTTokenServices;
        }





        public async Task<List<SubjectBySearchNameDTO>> SearchForSubject(string subjectName)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                var subjects = await _context.Subjects
               .Include(s => s.Files)
               .Include(s => s.User)
               .Include(s => s.GeneralSubjectsTypes)
               .Where(s => s.PrimarySubject.Contains(subjectName))
               .Select(s => new SubjectBySearchNameDTO
               {
                   Id = s.Id,
                   SubjectTypeName = s.SubjectTypeSubjects.Select(sts => sts.SubjectType.Name_Ar).ToList(),
                   PrimarySubject = s.PrimarySubject,
                   UsersImagesUrl = s.CommentSubjects.Where(cs => cs != null).Select(cs => cs.User.ImageURL).ToList(), // Assuming User has an ImageUrl property
                   CommentsCount = s.CommentSubjects.Count,
                   CreatedTime = s.CreatedTime,
                   File = s.Files.Select(f => new GetSubjectFilesDTO
                   {
                       FilePaths = f.FilePaths
                   }).FirstOrDefault(),

               }).ToListAsync();

                if (subjects == null)
                {
                    // If no subjects found, return an empty list
                    return new List<SubjectBySearchNameDTO>();
                }

                // Map the Subject entities to DTOs


                return subjects;
            }
            else
            {
                var subjects = await _context.Subjects
                .Include(s => s.Files)
                .Include(s => s.User)
                .Include(s => s.GeneralSubjectsTypes)
                .Where(s => s.PrimarySubject.Contains(subjectName))
                .Select(s => new SubjectBySearchNameDTO
                {
                    Id = s.Id,
                    SubjectTypeName = user.SelectedLanguage == "en" ? s.SubjectTypeSubjects.Select(sts => sts.SubjectType.Name_En).ToList() : s.SubjectTypeSubjects.Select(sts => sts.SubjectType.Name_Ar).ToList(),
                    PrimarySubject = s.PrimarySubject,
                    UsersImagesUrl = s.CommentSubjects.Where(cs => cs != null).Select(cs => cs.User.ImageURL).ToList(), // Assuming User has an ImageUrl property
                    CommentsCount = s.CommentSubjects.Count,
                    CreatedTime = s.CreatedTime,
                    File = s.Files.Select(f => new GetSubjectFilesDTO
                    {
                        FilePaths = f.FilePaths
                    }).FirstOrDefault(),

                }).ToListAsync();

                if (subjects == null)
                {
                    // If no subjects found, return an empty list
                    return new List<SubjectBySearchNameDTO>();
                }

                // Map the Subject entities to DTOs


                return subjects;
            }
            
        }


        //public async Task<List<SubjectBySearchNameDTO>> SearchForSubject(string subjectName)
        //{
        //     var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        //    var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
        //    var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
        //    var user = await _context.Users.FindAsync(userId);

        //    // Assuming dbContext is your EF DbContext instance
        //    var subjects = await _context.Subjects
        //        .Include(s => s.Files)
        //        .Include(s => s.User)
        //        .Include(cs=>cs.CommentSubjects)
        //        .Where(s => s.PrimarySubject.Contains(subjectName))
        //        .ToListAsync();

        //    // Map the Subject entities to DTOs
        //    var subjectDTOs = subjects.Select(s => new SubjectBySearchNameDTO
        //    {
        //        Id = s.Id,
        //        SubjectTypeName = user.SelectedLanguage == "en" ? s.SubjectTypeSubjects.Select(sts => sts.SubjectType.Name_En).ToList() : s.SubjectTypeSubjects.Select(sts => sts.SubjectType.Name_Ar).ToList(),
        //        PrimarySubject = s.PrimarySubject,
        //        UsersImagesUrl = s.CommentSubjects.Where(cs=> cs != null).Select(cs=> cs.User.ImageURL).ToList(), // Assuming User has an ImageUrl property
        //        CommentsCount = s.CommentSubjects.Count,
        //        CreatedTime = s.CreatedTime,
        //        File = new GetSubjectFilesDTO
        //        {
        //            // Assuming you want to map a specific file, you may adjust this part accordingly
        //            FilePaths = s.Files?.FirstOrDefault()?.FilePaths                    
        //        }
        //    }).ToList();

        //    return subjectDTOs;
        //}
    }
}
