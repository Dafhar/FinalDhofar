using DhofarAppApi.Dtos.User;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Data;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SendGrid.Helpers.Mail;
using SendGrid;
using PhoneNumbers;
using DhofarAppApi.Dtos.Subject;

namespace DhofarAppApi.Services
{
    public class UserServices : IUsers
    {
        private readonly AppDbContext _context;
        private readonly FileServices  _loadFile;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenService;
        public UserServices(AppDbContext context, UserManager<User> userManager, JWTTokenServices jWTTokenServices , IHttpContextAccessor httpContextAccessor, FileServices loadFile, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _loadFile = loadFile;
            _configuration = configuration; 
            _httpContextAccessor = httpContextAccessor;
            _jWTTokenService = jWTTokenServices;
        }
        public async Task<string> EditLanguage(string language)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _context.Users.FindAsync(userId);
            user.SelectedLanguage = language;
            await _context.SaveChangesAsync();
            return $"You change language to {language}";
        }

        public async Task<bool?> MuteSound()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Sound = !user.Sound;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return user.Sound;
            }
            return null;
        }

        public async Task<List<Colors>> GetAllColors()
        {
            var allColors = await _context.Colors.ToListAsync();

            return allColors;
        }

        public async Task<OperationResult> DeleteUser()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return OperationResult.Failed("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();

            if (result.Succeeded)
            {
                return OperationResult.Success();
            }
            else
            {
                // Handle errors
                // You can return specific error messages or handle them based on the result.Errors collection
                return OperationResult.Failed("Failed to delete user.");
            }
        }

        public async Task<ProfileDTO> EditUserProfile(EditProfileDTO editProfileDTO, ModelStateDictionary modelState)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _context.Users
                           .Include(s => s.Subjects)
                           .Include(c => c.CommentSubjects)
                           .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                modelState.AddModelError("", "User not found.");
                return null;
            }
            else
            {
                user.UserName = editProfileDTO.UserName;
                var existingUserWithPhoneNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == editProfileDTO.PhoneNumber);
                if (existingUserWithPhoneNumber != null && existingUserWithPhoneNumber.Id != userId)
                {
                    modelState.AddModelError("PhoneNumber", "Phone number is already in use by another user.");
                    return null;
                }

                if ((editProfileDTO.ImgrUrl != null || editProfileDTO.ImgrUrl != "") && editProfileDTO.ColorId==0)
                {
                    user.ImageURL = editProfileDTO.ImgrUrl;
                }

                 else if((editProfileDTO.ColorId != 0 && editProfileDTO.ColorId != null) && (editProfileDTO.ImgrUrl==null|| editProfileDTO.ImgrUrl == ""))
                {
                    var chosenColor = await _context.Colors.FindAsync(editProfileDTO.ColorId);
                    user.MyColor = chosenColor.HexColor;

                }
                user.Country = editProfileDTO.Country;
                user.FullName = editProfileDTO.FullName;
                user.Gender = editProfileDTO.Gender;
                user.Description = editProfileDTO.Description;
                user.PhoneNumber = user.CodeNumber+editProfileDTO.PhoneNumber;

                int? subjectCount = user.Subjects.Count;
                int? commentCount = user.CommentSubjects.Count;
                double rate = 0.0;

                // Calculate the total likes and dislikes for all subjects
                double totalLikes = user.Subjects.Sum(s => s.LikeCounter);
                double totalDislikes = user.Subjects.Sum(s => s.DisLikeCounter);

                // Avoid division by zero if there are no likes
                if (totalLikes > 0)
                {
                    rate = totalLikes / (totalLikes + totalDislikes);
                }

                var result = await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();

                if (result.Succeeded)
                {
                    return new ProfileDTO
                    {
                        
                        UserName = user.UserName,
                        Country = user.Country,
                        PhoneNumber = user.PhoneNumber,
                        FullName = user.FullName,
                        Gender = user.Gender,
                        Description = user.Description,
                        CountryCode = user.CodeNumber,
                        Color = user.MyColor,
                        File = user.ImageURL,
                        CommentCounter = commentCount,
                        SubjectCounter = subjectCount,
                        Rate = rate
                    };
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        var errorMessage = error.Code.Contains("UserName") ? nameof(editProfileDTO.UserName) :
                                           error.Code.Contains("FullName") ? nameof(editProfileDTO.FullName) :
                                           error.Code.Contains("Gender") ? nameof(editProfileDTO.Gender) :
                                           error.Code.Contains("PhoneNumber") ? nameof(editProfileDTO.PhoneNumber) :
                                           error.Code.Contains("Description") ? nameof(editProfileDTO.Description) :
                                           "";
                        modelState.AddModelError(errorMessage, error.Description);
                    }
                    return null;
                }
            }
        }

        //public async Task<User> GetUser()
        //{
        //    var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        //    var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
        //    var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

        //    var user = await _context.Users.Include(u=> u.UserColors).ThenInclude(u=>u.Colors).FirstOrDefaultAsync(u=> u.Id == userId);
        //    return user;
        //}
        public async Task<List<Colors>> GetColors()
        {
            return await _context.Colors.ToListAsync();
        }



        public async Task SendEmailAsync(string recipientEmail, string subject, string htmlContent)
        {
            string sendGridKey = _configuration["SendGrid:SendGridKey"];

            var client = new SendGridClient(sendGridKey);
            var from = new EmailAddress("your-email@example.com", "Your Name");
            var to = new EmailAddress(recipientEmail);
            var body = $"{htmlContent}";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, body);
            msg.HtmlContent = htmlContent;

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new Exception($"Failed to send email. Status code: {response.StatusCode}");
            }
        }


        public async Task<GetPersonalProfileDTO> GetPersonalProfile()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _context.Users
                .Include(s => s.Subjects)
                .Include(c => c.CommentSubjects)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }
            else
            {
                var subjectCount = user.Subjects.Count;
                var commentCount = user.CommentSubjects.Count;
                double rate = 0.0;

                // Calculate the total likes and dislikes for all subjects
                double totalLikes = user.Subjects.Sum(s => s.LikeCounter);
                double totalDislikes = user.Subjects.Sum(s => s.DisLikeCounter);

                // Avoid division by zero if there are no likes
                if (totalLikes > 0)
                {
                    rate = totalLikes / (totalLikes + totalDislikes);
                }
                string phoneNumberWithoutCountryCode = user.PhoneNumber.Substring(user.CodeNumber.Length);
                string trimmedCode = user.CodeNumber?.Trim();

                return new GetPersonalProfileDTO
                {
                    SubjectCounter = subjectCount,
                    CommentCounter = commentCount,
                    Rate = rate,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Code= trimmedCode,
                    PhoneNumber = phoneNumberWithoutCountryCode,
                    Country = user.Country,
                    Gender = user.Gender,
                    ImageUrl = user.ImageURL,
                    Color=user.MyColor,
                };
            }
        }
        public async Task<GetVisitorUserProfileDTO> GetVisitorUserProfile(string userId)
        {
            var user = await _context.Users
    .Include(s => s.Subjects)
        .ThenInclude(sts => sts.SubjectTypeSubjects)
        .ThenInclude(st=>st.SubjectType)
    .Include(c => c.CommentSubjects)
    .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            // Check if user.Subjects is null before accessing its Count property
            var subjectCount = user.Subjects?.Count ?? 0;

            // Check if user.CommentSubjects is null before accessing its Count property
            var commentCount = user.CommentSubjects?.Count ?? 0;

            double rate = 0.0;

            foreach (var item in user.Subjects)
            {
                if (item.LikeCounter + item.DisLikeCounter > 0)
                {
                    rate = (double)item.LikeCounter / (item.LikeCounter + item.DisLikeCounter);
                }
            }

            return new GetVisitorUserProfileDTO
            {
                SubjectCounter = subjectCount,
                CommentCounter = commentCount,
                Rate = rate,
                FullName = user.FullName,
                Description = user.Description,
                ImageUrl = user.ImageURL,
                Subjects = user.Subjects.Select(s => new GetSubjectForProfileDTO
                {
                    Id = s.Id,
                    SubjectType_Name = s.SubjectTypeSubjects.Select(sts => user.SelectedLanguage == "en" ? sts.SubjectType.Name_En : sts.SubjectType.Name_Ar).ToList(),
                    PrimarySubject = s.PrimarySubject,
                    ImageUrl = s.Files?.FirstOrDefault()?.FilePaths ?? "", // Handle possible null FilePaths
                    CommentCounter = s.CommentSubjects?.Count ?? 0, // Use null-conditional operator to avoid null reference exception
                    UserImages = s.CommentSubjects?.Select(cs => cs.User.ImageURL).ToList() ?? new List<string>(), // Use null-conditional operator to avoid null reference exception
                    CreatedTime = s.CreatedTime
                }).ToList()
            };

        }

        public async Task<List<ActiveUserDTO>> GetTopActiveUsers()
        {
            var topUsers = await _context.Users
            // Order users by the sum of subjects created, comments made, and votes given

            .OrderByDescending(u => u.Subjects.Count + u.CommentSubjects.Count + u.UserVotes.Count + u.RatingSubjects.Count)
            .Take(5) // Take the top 5 users
            .Select(u=> new ActiveUserDTO
            {
                FullName = u.FullName,
                ImageUrl = u.ImageURL
            })
            .ToListAsync();

            return topUsers;
        }
    }
}

