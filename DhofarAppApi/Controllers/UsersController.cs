using DhofarAppApi.Dtos.User;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsers _users;
        public UsersController(IUsers users)
        {
            _users = users;
        }


        [HttpPost("EditLanguage")]
        public async Task<IActionResult> EditLanguage(LanguageDTO language)
        {
            var result = await _users.EditLanguage(language.language);
            return Ok(result);
        }


        [Authorize]
        [HttpPut("EditProfile")]
        public async Task<IActionResult> EditProfile( EditProfileDTO editProfileDTO)
        {
            var user = await _users.EditUserProfile( editProfileDTO, this.ModelState);
            if (ModelState.IsValid)
            {
                if (user != null)
                    return Ok(user);
                else
                    return BadRequest();
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        [Authorize(Roles = "Super Admin, Admin, User")]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser()   
        {
            var result = await _users.DeleteUser();
            if (result.SuccessMessage)
            {
                return Ok("User deleted successfully.");
            }
            else
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("Colors")]
        public async Task<ActionResult<List<Colors>>> GetColors()
        {
            var colors = await _users.GetAllColors();
            if (colors == null)
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }
            else
                return colors;
        }

        [HttpPut("Mute")]
        public async Task<IActionResult> Mute()
        {
            var result = await _users.MuteSound();
            if (result == true)
            {
                return Ok("Sound On");
            }
            else if (result == false)
            {
                return Ok("Sound Off");
            }
            else
            {
                return NotFound("User not found");
            }

        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var user = await _users.GetPersonalProfile();
            if (user == null)
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }

            return Ok(user);
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromForm] string recipientEmail, [FromForm] string subject, [FromForm] string htmlContent)
        {
            try
            {
                await _users.SendEmailAsync(recipientEmail, subject, htmlContent);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }
        }

        [HttpGet("GetAllColors")]
        public async Task<IActionResult> GetAllColors()
        {
            var colors = await _users.GetAllColors();
            return Ok(colors);
        }
        [HttpGet("personal-profile")]
        public async Task<ActionResult<GetPersonalProfileDTO>> GetPersonalProfile()
        {
            var personalProfile = await _users.GetPersonalProfile();
            if (personalProfile == null)
            {
                return NotFound();
            }
            return personalProfile;
        }
        [HttpGet("VisitorProfile/{userId}")]
        public async Task<IActionResult> GetUserProfilePublic(string userId)
        {
            var user = await _users.GetVisitorUserProfile(userId);
            if (user == null)
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }

            return Ok(user);
        }
    }
}
