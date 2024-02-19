using DhofarAppApi.Dtos.User;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace DhofarAppApi.InterFaces
{
    public interface IUsers
    {
        public Task<string> EditLanguage(string language);

       
        public Task<ProfileDTO> EditUserProfile(EditProfileDTO editProfileDTO, ModelStateDictionary modelState);

        public Task<bool?> MuteSound();

        public Task<List<Colors>> GetAllColors();

        public Task<OperationResult> DeleteUser();

        // public Task<User> GetUser();

        public Task SendEmailAsync(string recipientEmail, string subject, string htmlContent);

        Task<GetVisitorUserProfileDTO> GetVisitorUserProfile(string userId);

        Task<GetPersonalProfileDTO> GetPersonalProfile();

        Task<List<ActiveUserDTO>> GetTopActiveUsers();





    }
}
