using DhofarAppApi.Data;
using DhofarAppApi.Dtos.User;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using DhofarAppApi.Dtos.User;
using PhoneNumbers;
using DhofarAppApi.Services;
using System.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;




namespace DhofarAppApi.Services
{
    public class IdentityUserServices : IIDentityUser
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JWTTokenServices _jWTTokenService;
        private readonly IStringLocalizer<IdentityUserServices> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AppDbContext _context;
        private string SecretKey = "$2y$10$.vBG561wmRjrwAVXD98HNOgsNpDczlqm3Jq9QnEd1rVAGv3Fykk1a";

        private readonly ResourceManager _resourceManager;

        public IdentityUserServices(SignInManager<User> signInManager, IStringLocalizer<IdentityUserServices> localizer, IHttpContextAccessor httpContextAccessor , UserManager<User> userManager, JWTTokenServices jWTTokenService, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jWTTokenService = jWTTokenService;
            _localizer = localizer;
            _context = context;
            _resourceManager = new ResourceManager("DhofarAppApi.Resources.ErrorMessages", typeof(IdentityUserServices).Assembly);
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<UserDTO> Register(RegisterDTO model, ModelStateDictionary modelState, ClaimsPrincipal principal)
        {
            var lang = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString().Split(',')[0].Trim();

            var lastIdentityNumber = await _context.IdentityNumbers.FirstOrDefaultAsync();
            int generateIdentityId = (lastIdentityNumber != null) ? lastIdentityNumber.Value : 1;

            var user = new User()
            {
                UserName = model.UserName,
                FullName = model.Fullname,
                Email = model.Email,
                CodeNumber = model.CodeNumber,
                PhoneNumber = $"{model.CodeNumber}{model.PhoneNumber}",
                IdentityNumber = generateIdentityId++,
                JoinedDate = DateTime.UtcNow,
                SelectedLanguage=lang=="en"?lang:"ar"
            };


            // Check if the CodeNumber is unique
            var phoneNumberExist = await _userManager.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber);
            if (phoneNumberExist)
            {
                modelState.AddModelError(nameof(model.PhoneNumber), "PhoneNumber must be unique.");
                return null;
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                IList<string> role = new List<string>() { "User" };
                await _userManager.AddToRolesAsync(user, role);

                if (lastIdentityNumber != null)
                {
                    lastIdentityNumber.Value = generateIdentityId;
                }
                else
                {
                    await _context.IdentityNumbers.AddAsync(new IdentityNumber { Value = generateIdentityId });
                }
                await _context.SaveChangesAsync();
                //var usertoken = new DeviceToken()
                //{
                //    UserId = user.Id,
                //    Token = from mobile
                //};

                //await _context.deviceTokens.AddAsync(usertoken);

                return new UserDTO
                {
                    Id = user.Id,
                    IdentityNumber = user.IdentityNumber,
                    UserName = user.UserName,
                    Token = await _jWTTokenService.GetToken(user, TimeSpan.FromHours(2)),
                    Roles = await _userManager.GetRolesAsync(user),
                };
            }
            else
            {
                if (lang=="ar")
                {
                    foreach (var error in result.Errors)
                    {
                        var errorCode = error.Code;
                        var errorMessage = _resourceManager.GetString($"{errorCode}_{user.SelectedLanguage}") ?? error.Description;
                        modelState.AddModelError(errorCode, errorMessage);
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        var errorMessage = error.Code.Contains("Email") ? nameof(model.Email) :
                                           error.Code.Contains("UserName") ? nameof(model.UserName) :
                                           error.Code.Contains("Fullname") ? nameof(model.Fullname) :
                                           error.Code.Contains("CodeNumber") ? nameof(model.CodeNumber) :
                                           error.Code.Contains("PhoneNumber") ? nameof(model.PhoneNumber) :
                                           error.Code.Contains("Password") ? nameof(model.Password) :
                                           error.Code.Contains("ConfirmPassword") ? nameof(model.ConfirmPassword) :
                                           "";
                        modelState.AddModelError(errorMessage, error.Description);
                    }
                }
                
                return null;
                
            }
        }


        public async Task<LoginDTO>Login(LoginInputDTO loginInputDTO)
        {

            var userByPhoneNumber = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == loginInputDTO.PhoneNumber);
            if (userByPhoneNumber == null)
            {
                return null;    
            }
            bool isValidPassword = await _userManager.CheckPasswordAsync(userByPhoneNumber, loginInputDTO.Password);
                if (isValidPassword)
                {
                    userByPhoneNumber.LogInDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    return new LoginDTO
                    {
                        PhoneNumber= userByPhoneNumber.PhoneNumber,
                        UserName = userByPhoneNumber.UserName,
                        Token = await _jWTTokenService.GetToken(userByPhoneNumber, TimeSpan.FromHours(2)),
                        Roles = await _userManager.GetRolesAsync(userByPhoneNumber)
                    };
                 }

            return null;
        }
        public async Task Logout(ClaimsPrincipal principal)
        {
            await _signInManager.SignOutAsync();
        }



        public async Task<JsonResult> SendOtpToRegister(phoneNumberDTO phoneNumberdto)
        {
            var userByPhoneNumber = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumberdto.PhoneNumber);
            if (userByPhoneNumber!=null)
            {
                var errorResponse = new
                {
                    errors = new
                    {
                        message = "User Already Registered"
                    }
                };

                // Return the error response as JSON
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // Internal Server Error
                };
            }
            try
            {
                string randOtp = "1234"; // new Random().Next(1000, 9999).ToString();
                string datetime = DateTime.Now.ToString();

                var obj = new
                {
                    phoneNumber = phoneNumberdto.PhoneNumber,
                    hashString = BCrypt.Net.BCrypt.HashPassword(SecretKey + phoneNumberdto.PhoneNumber + randOtp + datetime),
                    createdAt = datetime,
                };

                // Return the response as JSON
                return new JsonResult(obj);
            }
            catch (Exception ex)
            {
                
                var errorResponse = new
                {
                    errors = new
                    {
                        message = "An error occurred while sending OTP to register."
                    }
                };

                // Return the error response as JSON
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // Internal Server Error
                };
            }
        }

        public async Task<JsonResult> VerifyOtp(VerifyOtpDto verifyOtpDto)
        {
            try
            {
                // Concatenate string values for hashing
                var dataToHash = SecretKey + verifyOtpDto.phoneNumber + verifyOtpDto.otpCode + verifyOtpDto.createdAt;

                // Verify the hash
                var success = BCrypt.Net.BCrypt.Verify(dataToHash, verifyOtpDto.hashString);

                // Create the response object
                var responseObject = new
                {
                    Success = success,
                };

                // Return the response as JSON
                return new JsonResult(responseObject);
            }
            catch (Exception ex)
            {
                // Log the exception

                // Create the error response object
                var errorResponse = new
                {
                    errors = new
                    {
                        message = "An error occurred while verifying OTP."
                    }
                };

                // Return the error response as JSON
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // Internal Server Error
                };
            }
        }


        public async Task<string> ResetPassword(PasswordDTO loginInputDTO)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _context.Users.FindAsync(userId); bool isValidPassword = await _userManager.CheckPasswordAsync(user, loginInputDTO.Password);
           
            if (user != null)
            {
                var result = await _userManager.RemovePasswordAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddPasswordAsync(user, loginInputDTO.Password);

                    if (result.Succeeded)
                    {
                        return "success";
                    }
                    else
                    {
                        return "invalid password";
                    }

                }
            }
            return "User Not Found";
        }

        public async Task<string> UpdatePassword(UpdatePasswordDTO updatePassword)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return ("User Not Found");
            }

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, updatePassword.OldPassword);

            if (!isValidPassword)
            {
                return ("OldPassword Incorrect");
            }

            var result = await _userManager.RemovePasswordAsync(user);

            if (!result.Succeeded)
            {
                return ("Failed to remove old password");
            }

            result = await _userManager.AddPasswordAsync(user, updatePassword.NewPassword);
            await _context.SaveChangesAsync();

            if (!result.Succeeded)
            {
                return ("Failed to set new password");
            }

            return "Success";
        }

        public async Task<List<Country>> GetCountry()
        {
            var countries = await _context.Countries.ToListAsync();
            return countries;
        }

        public async Task<JsonResult> SendOtpToResetPassWord(phoneNumberDTO phoneNumberDTO)
        {
            try
            {
                var userByPhoneNumber = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumberDTO.PhoneNumber);
                if (userByPhoneNumber == null)
                {
                    // User not found, return an error response
                    var errorResponse = new
                    {
                        errors = new
                        {
                            message = "User not found"
                        }
                    };
                    return new JsonResult(errorResponse)
                    {
                        StatusCode = 404 // Not Found
                    };
                }

                // Generate OTP and hash the data
                string randOtp = "1234"; // new Random().Next(1000, 9999).ToString();
                string datetime = DateTime.Now.ToString();

                var obj = new
                {
                    phoneNumber = phoneNumberDTO.PhoneNumber,
                    hashString = BCrypt.Net.BCrypt.HashPassword(SecretKey + phoneNumberDTO.PhoneNumber + randOtp + datetime),
                    createdAt = datetime,
                };

                // Return the response as JSON
                return new JsonResult(obj);
            }
            catch (Exception ex)
            {

                // Create the error response object
                var errorResponse = new
                {
                    errors = new
                    {
                        message = "An error occurred while sending OTP to reset password."
                    }
                };

                // Return the error response as JSON
                return new JsonResult(errorResponse)
                {
                    StatusCode = 500 // Internal Server Error
                };
            }
        }
    }
}

