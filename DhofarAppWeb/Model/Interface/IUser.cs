using DhofarAppWeb.Dtos.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DhofarAppWeb.Model.Interface
{
    public interface IUser
    {
        public Task<UserDTO> Register(RegisterDTO registerUser, ModelStateDictionary modelState);

        Task<UserDTO> Login(LoginInputDTO loginInputDTO, ModelStateDictionary modelState);
    }
}
