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
using DhofarAppApi.Dtos.GeneralSubject;
using DhofarAppApi.Dtos.SubjectFiles;
using DhofarAppApi.Dtos.Comment;
using DhofarAppApi.Dtos.ReplyComment;

namespace DhofarAppWeb.Services
{
    public class GeneralSubjectTypeService : IGeneralSubject
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenServices;

        public GeneralSubjectTypeService(AppDbContext db, IHttpContextAccessor httpContextAccessor, JWTTokenServices jWTTokenServices)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _jWTTokenServices = jWTTokenServices;
        }

        public async Task<List<GetGeneralSubjectTypeToAllDTO>> GetAllSubjectTypesForAll()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);

            var generalSubjectTypes = await _db.GeneralSubjectsTypes
            .Where(gs => gs.Title_En == "public")
            .Include(gs => gs.Subjects)
                .ThenInclude(s => s.CommentSubjects)
            .ToListAsync();

            var mappedResults = generalSubjectTypes.Select(st => new GetGeneralSubjectTypeToAllDTO
            {
                Id = st.Id,
                Name = user.SelectedLanguage == "en" ? st.Name_En : st.Name_Ar,
                SubjectsCounter = st.Subjects.Count,
                CommentsCounter = st.Subjects.SelectMany(s => s.CommentSubjects).Count(), // Count all comment subjects across all subjects
                TopUsersImages = st.Subjects.Where(s => s.User != null).Select(s => s.User.ImageURL).Take(5).ToList(), // Add null check for User property
            }).ToList();


            return mappedResults;

            //Subjects = st.Subjects.Select(s => new GetSubjectDTO
            //{
            //    Id = s.Id,
            //    Title= s.Title,
            //    PrimarySubject = s.PrimarySubject,
            //    SubjectType_Name = s.SubjectTypeSubjects.Select(sts => user.SelectedLanguage == "en"?  sts.SubjectType.Name_En: sts.SubjectType.Name_Ar).ToList(),
            //    CreatedTime = s.CreatedTime,
            //    UserName = s.User.UserName,
            //    UserImageUrl = s.User.ImageURL,
            //    Description = s.Description,
            //    Files = s.Files.Select(f=> new GetSubjectFilesDTO
            //    {
            //        FilePaths = f.FilePaths
            //    }).ToList(),
            //    CommentsSubjects = s.CommentSubjects.Select(cs=> new GetSubjectCommentDTO
            //    {
            //       Comment =cs.Comment,
            //       CommentTime = cs.CommentTime,
            //       UserName = cs.User.UserName,
            //       UserImageUrl=  cs.User.ImageURL,
            //       File = cs.File,
            //       ReplyComments = cs.CommentReplies.Select(cr=> new GetReplyCommentDTO
            //       {
            //           UserImageUrl = cr.User.ImageURL,
            //           UserName = cr.User.UserName,
            //           ReplyComment = cr.ReplyComment,
            //           file = cr.File
            //       }).ToList()

            //    }).ToList(),
            //    CommentsCount = s.CommentSubjects.Count

            //}).ToList()

        }

        public async Task<GetSinlgeGeneralSubjectTypeDTO> GetSubjectTypeById(int id)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);


            var subjectType = await _db.GeneralSubjectsTypes
                .Where(st => st.Id == id)
                .Select(st => new GetSinlgeGeneralSubjectTypeDTO
                {
                    Id =st.Id,
                    Name = user.SelectedLanguage == "en" ? st.Name_En : st.Name_Ar,
                    Subjects = st.Subjects.Select(s=> new ListOfAllSubjectByGeneralSubjectTypeDTO
                    {
                        Id = s.Id,
                        SubjectTypeName = s.SubjectTypeSubjects.Select(sts => user.SelectedLanguage == "en" ? sts.SubjectType.Name_En : sts.SubjectType.Name_Ar).ToList(),
                        PrimarySubject = s.PrimarySubject,
                        UsersImagesUrl = st.Subjects.Where(s => s.CommentSubjects != null).Select(s => s.User.ImageURL).Distinct().ToList(), // Add null check for User property
                        CommentsCount = s.CommentSubjects.Count,
                        CreatedTime = s.CreatedTime,
                        File = s.Files.Select(f => new GetSubjectFilesDTO
                        {
                            FilePaths = f.FilePaths
                        }).FirstOrDefault(),
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return subjectType;
        }

        public async Task<GeneralSubjectsType> CreateSubjectType(PostGeneralSubjectTypeDTO postSubjectTypeDto)
        {
            var subjectType = new GeneralSubjectsType
            {
                Name_En = postSubjectTypeDto.Name_En,
                Name_Ar = postSubjectTypeDto.Name_Ar,
                Title_Ar = postSubjectTypeDto.Title_Ar,
                Title_En = postSubjectTypeDto.Title_En
            };

            await _db.GeneralSubjectsTypes.AddAsync(subjectType);
            await _db.SaveChangesAsync();

            return subjectType;
        }

        public async Task<GeneralSubjectsType> UpdateSubjectType(int id, PostGeneralSubjectTypeDTO postSubjectTypeDto)
        {
            var subjectType = await _db.GeneralSubjectsTypes.FindAsync(id);

            if (subjectType == null)
                return null;

            subjectType.Name_Ar = postSubjectTypeDto.Name_Ar;
            subjectType.Name_En = postSubjectTypeDto.Name_En;
            subjectType.Title_Ar = postSubjectTypeDto.Title_Ar;
            subjectType.Title_En = postSubjectTypeDto.Title_En;

            _db.Entry(subjectType).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return subjectType;
        }

        public async Task<bool> DeleteSubjectType(int id)
        {
            var subjectType = await _db.GeneralSubjectsTypes.FindAsync(id);

            if (subjectType == null)
                return false;

            _db.GeneralSubjectsTypes.Remove(subjectType);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<GetGeneralSubjectTypeDTO>> GetAllSubjectTypesForAdmin()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);

            var generalSubjectTypes = await _db.GeneralSubjectsTypes
    .Include(gs => gs.Subjects)
        .ThenInclude(s => s.CommentSubjects)
    .ToListAsync();


            var mappedResults = generalSubjectTypes.Select(st => new GetGeneralSubjectTypeDTO
            {
                Id = st.Id,
                Name = user.SelectedLanguage == "en" ? st.Name_En : st.Name_Ar,
                Title = user.SelectedLanguage != "en" ? st.Title_En : st.Title_Ar,
                SubjectsCounter = st.Subjects.Count,
                CommentsCounter = st.Subjects.SelectMany(s => s.CommentSubjects).Count(), // Count all comment subjects across all subjects
                TopUsersImages = st.Subjects.Where(s => s.User != null).Select(s => s.User.ImageURL).Take(5).ToList(), // Add null check for User property
            }).ToList();

            return mappedResults;
        }
    }
}
