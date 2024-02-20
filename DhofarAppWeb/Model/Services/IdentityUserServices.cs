using DhofarAppWeb.Data;
using DhofarAppWeb.Dtos.User;
using DhofarAppWeb.Model.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Resources;

namespace DhofarAppWeb.Model.Services
{
    public class IdentityUserServices : IUser
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        //private readonly ResourceManager _resourceManager;



        public IdentityUserServices(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<UserDTO> Register(RegisterDTO registerUser, ModelStateDictionary modelState)
        {
            var lang = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString().Split(',')[0].Trim();

            var lastIdentityNumber = await _context.IdentityNumbers.FirstOrDefaultAsync();
            int generateIdentityId = (lastIdentityNumber != null) ? lastIdentityNumber.Value : 1;

            var user = new User()
            {
                UserName = registerUser.UserName,
                FullName = registerUser.Fullname,
                Email = registerUser.Email,
                CodeNumber = registerUser.CodeNumber,
                PhoneNumber = $"{registerUser.CodeNumber}{registerUser.PhoneNumber}",
                IdentityNumber = generateIdentityId++,
                JoinedDate = DateTime.UtcNow,
                SelectedLanguage = lang == "en" ? lang : "ar"
            };


            // Check if the CodeNumber is unique
            var phoneNumberExist = await _userManager.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber);
            if (phoneNumberExist)
            {
                modelState.AddModelError(nameof(registerUser.PhoneNumber), "PhoneNumber must be unique.");
                return null;
            }
            if (registerUser.Password == null)
            {
                return null;
            }
            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
            {
                string roles = "User";
                var roleExists = await _roleManager.RoleExistsAsync(roles);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roles));
                }

                await _userManager.AddToRoleAsync(user, roles);

                if (lastIdentityNumber != null)
                {
                    lastIdentityNumber.Value = generateIdentityId;
                }
                else
                {
                    await _context.IdentityNumbers.AddAsync(new IdentityNumber { Value = generateIdentityId });
                }
                await _context.SaveChangesAsync();
               

                return new UserDTO
                {
                    Id = user.Id,
                    IdentityNumber = user.IdentityNumber,
                    UserName = user.UserName,
                    Roles = await _userManager.GetRolesAsync(user),
                };
            }
            else
            {
                
                    foreach (var error in result.Errors)
                    {
                        var errorMessage = error.Code.Contains("Email") ? nameof(registerUser.Email) :
                                           error.Code.Contains("UserName") ? nameof(registerUser.UserName) :
                                           error.Code.Contains("Fullname") ? nameof(registerUser.Fullname) :
                                           error.Code.Contains("CodeNumber") ? nameof(registerUser.CodeNumber) :
                                           error.Code.Contains("PhoneNumber") ? nameof(registerUser.PhoneNumber) :
                                           error.Code.Contains("Password") ? nameof(registerUser.Password) :
                                           error.Code.Contains("ConfirmPassword") ? nameof(registerUser.ConfirmPassword) :
                                           "";
                        modelState.AddModelError(errorMessage, error.Description);
                    }
                

                return null;

            }
           
        }

        public async Task<UserDTO> Login(LoginInputDTO loginInputDTO, ModelStateDictionary modelState)
        {
            var userByPhoneNumber = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == loginInputDTO.LoginPhoneNumber);
            if (userByPhoneNumber != null)
            {
                bool isValidPassword = await _userManager.CheckPasswordAsync(userByPhoneNumber, loginInputDTO.LoginPassword);
                if (isValidPassword)
                {
                    userByPhoneNumber.LogInDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    var result = await _signInManager.PasswordSignInAsync(userByPhoneNumber.UserName, loginInputDTO.LoginPassword, true, false);
                    if (result.Succeeded)
                    {
                        return new UserDTO
                        {
                            Id = userByPhoneNumber.Id,
                            IdentityNumber = userByPhoneNumber.IdentityNumber,
                            UserName = userByPhoneNumber.UserName,
                            Roles = await _userManager.GetRolesAsync(userByPhoneNumber)
                        };
                    }
                    else
                    {
                        modelState.AddModelError(string.Empty, "Username or password wrong");
                        return null;
                    }
                }
                else
                {
                    return null;
                }
               
            }
          

            return null;
        }
    }
}

