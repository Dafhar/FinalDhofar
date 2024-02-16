using DhofarAppApi.Dtos.Category;
using DhofarAppApi.Dtos.Complaint;
using DhofarAppApi.Dtos.DepartmentType;
using DhofarAppApi.Dtos.SubCategory;
using DhofarAppApi.Dtos.User;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DhofarAppApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _adimn;

        public AdminController(IAdmin admin)
        {
            _adimn=admin;
        }
        [Authorize(Roles = "Super Admin")]
        [HttpPost("RegisterEmployee")]
        public async Task<ActionResult<UserDTO>> RegisterEmployee(RegisterEmployeeDTO registerDTO)
        {
            var user = await _adimn.RegisterEmployee(registerDTO, this.ModelState, User);
            if (ModelState.IsValid)
            {
                if (user != null)
                    return user;

                else
                    return NotFound();
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        [Authorize(Roles = "Super Admin")]
        [HttpPost("RegisterSuperAdmin")]
        public async Task<ActionResult<UserDTO>> RegisterSuperAdmin(RegisterSuperAdminDTO registerDTO)
        {
            var user = await _adimn.RegisterSuperAdmin(registerDTO, this.ModelState, User);
            if (ModelState.IsValid)
            {
                if (user != null)
                    return user;

                else
                    return NotFound();
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }



        [HttpGet("TopFiveCommenters")]
        public async Task<IActionResult> GetTopFiveCommenters()
        {
            var topFiveCommenters = await _adimn.GetTopFiveCommenters();
            return Ok(topFiveCommenters);
        }

        [HttpGet("UserStatisticsPerDay")]
        public async Task<IActionResult> GetVisitorStatisticsPerDay()
        {
            var (usersPerDay, totalUsers) = await _adimn.GetUserStaticsPerDay();
            return Ok(new { UsersPerDay = usersPerDay, TotalUsers = totalUsers });
        }


        [Authorize(Roles = "Super Admin, Admin")]
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _adimn.DeleteUser(userId);
            if (result.SuccessMessage)
            {
                return Ok("User deleted successfully.");
            }
            else
            {
                return NotFound("User not found.");
            }
        }

        [Authorize(Roles = "Super Admin")]
        [HttpPost("ChangeRole")]
        // TODO: Need DTO 
        public async Task<IActionResult> ChangeUserRole(string userId, ChangeRoleDTO changeRole)
        {
            var result = await _adimn.ChangeUserRole(userId, changeRole.CurrentRole, changeRole.NewRole);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Cannot Change the role for this User");
            }
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            var user = await _adimn.GetUser(userName);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("UserstatisticsPerMonth")]
        public async Task<IActionResult> GetVisitorStatisticsPerMonth()
        {
            var loginStatistics = await _adimn.GetUserStaticsPerMonth();
            return Ok(loginStatistics);
        }



        [HttpGet("VisitorCounter")]
        public async Task<IActionResult> GetVisitorCounter()
        {
            var counter = await _adimn.GetUsersCount();

            return Ok(new { UserCounter = counter });
        }

        [HttpGet("Count")]
        public async Task<IActionResult> GetUsersCount()
        {
            var count = await _adimn.GetUsersCount();
            return Ok(count);
        }

        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adimn.GetAllUser();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<CreatCategortDTO>> CreateCategory(CreatCategortDTO category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCategory = await _adimn.CreateCategort(category);
            return Ok(createdCategory);
        }

        // PUT: api/Category/5
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateCategory(int Id, [FromBody] UpdateCategoryDTO category)
        {

            var updatedCategory = await _adimn.UpdateCategory(Id, category);

            if (updatedCategory == null)
            {

                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }

            return NoContent();
        }

        // DELETE: api/Category/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            await _adimn.DeleteCategort(Id);
            return NoContent();
        }

        [HttpPost("{categoryId}/Subcategory")]
        public async Task<ActionResult<CreateSubCategoryDTO>> CreateSubcategory(int categoryId, CreateSubCategoryDTO subCategory)
        {
            var createdSubcategory = await _adimn.CreateSubcategory(categoryId, subCategory);
            if (createdSubcategory == null)
            {

                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }

            return Ok(createdSubcategory);
        }

        [HttpPut("{categoryId}/Subcategory/{subCategoryId}")]
        public async Task<IActionResult> UpdateSubcategory(int categoryId, int subCategoryId, CreateSubCategoryDTO subCategory)
        {
            var updatedSubcategory = await _adimn.UpdateSubcategory(categoryId, subCategoryId, subCategory);
            if (updatedSubcategory == null)
            {

                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }

            return Ok();
        }

        [HttpDelete("{categoryId}/Subcategory/{subCategoryId}")]
        public async Task<IActionResult> DeleteSubcategory(int categoryId, int subCategoryId)
        {
            await _adimn.DeleteSubcategory(categoryId, subCategoryId);
            return NoContent();
        }




        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUserComplaint(string userId)
        {
            var complaints = await _adimn.GetAllUserComplaint(userId);
            return Ok(complaints);
        }


        [HttpGet("GetAllAdmin")]
        public async Task<IActionResult> GetAllComplaintAdmin()
        {
            var complaints = await _adimn.GetAllComplaint();
            return Ok(complaints);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteComplaint(int id, string why)
        {
            var result = await _adimn.DeleteComplaint(id, why);

            if (result == "Deleted successfully")
            {
                return Ok(result);
            }
            else if (result == "Not Found")
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }
            else
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditComplaintStatus(int id, EditComplaintStatus status)
        {
            var result = await _adimn.EditComplaintStatus(id, status);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("allByDepartment/{departmentTypeId}")]
        public async Task<IActionResult> GetAllComplaintByDepartment(int departmentTypeId)
        {
            var result = await _adimn.GetAllComplaintByDepartment(departmentTypeId);
            return Ok(result);
        }

        [HttpGet("byIdAndDepartment/{complaintId}/{departmentTypeId}")]
        public async Task<IActionResult> GetComplaintByIdAndDepartment(int complaintId, int departmentTypeId)
        {
            var result = await _adimn.GetComplaintByIdAndDepartment(complaintId, departmentTypeId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("closedInDepartment/{id}")]
        public async Task<IActionResult> ClosedInDepartment(int id)
        {
            var result = await _adimn.ComplaintClosedinDpartment(id);
            return Ok(result);
        }

        [HttpPut("notMySpecialist/{id}")]
        public async Task<IActionResult> ComplaintNotMySpecialist(int id, NotMySpecialistComplaint complaint)
        {
            var result = await _adimn.ComplaintNotMySpecialist(id, complaint);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("allClosed")]
        public async Task<IActionResult> AllClosedComplaint()
        {
            var result = await _adimn.AllClosedComplaint();
            return Ok(result);
        }

        [HttpGet("allRejected")]
        public async Task<IActionResult> AllRejectedComplaint()
        {
            var result = await _adimn.AllRejectedComplaint();
            return Ok(result);
        }
        //TODO : new Dto 
        [HttpPut("editDepartment/{id}")]
        public async Task<IActionResult> EditDepartment(int id, ChangeDepartmentDTO departmentDTO)
        {
            var result = await _adimn.EditDepartment(id, departmentDTO.DepartmentId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }
        }

        [HttpPut("accept/{id}")]
        public async Task<IActionResult> AcceptedComplaint(int id, AcceptedComplaint complaint)
        {
            var result = await _adimn.AcceptedComplaint(id, complaint);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }
        }




    }
}
