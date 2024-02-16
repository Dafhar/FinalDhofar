using DhofarAppApi.Dtos.User;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Services;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIDentityUser _context;

        public AuthController(IIDentityUser context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var user = await _context.Register(registerDTO, this.ModelState, User);
            if (ModelState.IsValid)
            {
                if (user != null)
                    return user;

                else
                    return NotFound();
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }



        [HttpPost("Login")]
        public async Task<ActionResult<LoginDTO>> Login(LoginInputDTO loginDto)
        {
            var user = await _context.Login(loginDto);

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    return user;
                }
                else
                {
                    var errorResponse = new { errors = new { message = "Invalid phone number or password." } };
                    return BadRequest(errorResponse);
                }
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        //TODO: new DTO
        [HttpPost("ForgetPassword")]
        public async Task<ActionResult<UserDTO>> ForgetPassword(PasswordDTO forgetPasswordDTO)
        {
            var result = await _context.ResetPassword(forgetPasswordDTO);
            if (result == "success")
            {
                return Ok("Password reset successfully.");
            }
            else
            {
                var errorResponse = new { errors = new { message = result } };
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult<UserDTO>> UpdatePassword(UpdatePasswordDTO updatePassword )
        {
            var result = await _context.UpdatePassword(updatePassword);
            if (result == "Success")
            {
                return Ok("Password Updated successful.");
            }
            else
            {
                var errorResponse = new { errors = new { message = result } };
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("SendOtp")]
        public async Task<JsonResult> SendOtpToRegister(phoneNumberDTO phoneNumberDTO)
        {
            var result = await _context.SendOtpToRegister(phoneNumberDTO);
            return new JsonResult(result);
        }

        [HttpPost("VerifyOtp")]
        public async Task<JsonResult> VerifyOtp(VerifyOtpDto verifyOtpDto)
        {
             var result = await _context.VerifyOtp(verifyOtpDto);

            return result;
        }



        [HttpGet]
        public async Task<ActionResult<List<Country>>> GetCountry()
        {
            var countries = await _context.GetCountry();
            return countries;
        }

        [HttpPost]
        public async Task<JsonResult> SendOtpToResetPassWord(phoneNumberDTO phoneNumberDTO)
        {
            var result = await _context.SendOtpToResetPassWord(phoneNumberDTO);
            return result;
        }



    }
}
