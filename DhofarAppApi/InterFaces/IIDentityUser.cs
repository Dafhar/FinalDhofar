using DhofarAppApi.Dtos.User;
using DhofarAppApi.Dtos.User;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace DhofarAppApi.InterFaces
{
    public interface IIDentityUser
    {
        public Task<UserDTO> Register([FromForm] RegisterDTO model, ModelStateDictionary modelState, ClaimsPrincipal principal);
        public Task<LoginDTO> Login(LoginInputDTO loginInputDTO);
        public Task<string> ResetPassword(PasswordDTO loginInputDTO);
        public Task<string> UpdatePassword(UpdatePasswordDTO updatePassword);

        public Task Logout(ClaimsPrincipal principal);
        public Task<JsonResult> SendOtpToRegister(phoneNumberDTO phoneNumber);
        public Task<JsonResult> SendOtpToResetPassWord(phoneNumberDTO phoneNumberDTO);
        public Task<JsonResult> VerifyOtp(VerifyOtpDto verifyOtpDto);
        public Task<List<Country>> GetCountry();





    }
}
