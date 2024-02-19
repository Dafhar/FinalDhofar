using DhofarAppApi.Dtos.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DhofarAppApi.Model;
using DhofarAppApi.Dtos.SubCategory;
using DhofarAppApi.Dtos.Category;
using DhofarAppApi.Dtos.Complaint;

namespace DhofarAppApi.InterFaces
{
    public interface IAdmin
    {
        public Task<UserDTO> ChangeUserRole(string userId, string oldRole, string newRole);

        public Task<OperationResult> DeleteUser(string userId);

        public Task<Dictionary<string, int>> GetUserStaticsPerMonth();
        public Task<List<(UserDTO User, int SubjectCount, int CommentCount)>> GetTopFiveCommenters();

        public Task<int> GetUsersCount();

        public Task<(List<object>, int)> GetUserStaticsPerDay();

        public Task<GetUserDTO> GetUser(string UserName);
        public Task<List<GetUserDTO>> GetAllUser();



        public Task<UserDTO> RegisterSuperAdmin([FromBody] RegisterSuperAdminDTO model, ModelStateDictionary modelState, ClaimsPrincipal principal);


        public Task<UserDTO> RegisterEmployee([FromBody] RegisterEmployeeDTO model, ModelStateDictionary modelState, ClaimsPrincipal principal);


        public Task<CreateSubCategoryDTO> CreateSubcategory(int categoryId, CreateSubCategoryDTO subCategory);
        public Task<CreateSubCategoryDTO> UpdateSubcategory(int categoryId, int subCategoryId, CreateSubCategoryDTO subCategory);
        public Task DeleteSubcategory(int categoryId, int subCategoryId);

        public Task<CreatCategortDTO> CreateCategort(CreatCategortDTO category);
        public Task<UpdateCategoryDTO> UpdateCategory(int id, UpdateCategoryDTO updatecategory);
        public Task DeleteCategort(int Id);



        public Task<List<GetAllComplaintAdmin>> GetAllComplaint();
        public Task<List<GetComplaintDTO>> GetAllUserComplaint();
        public Task<string> DeleteComplaint(int Id, string why);
        public Task<EditComplaintStatus> EditComplaintStatus(int Id, EditComplaintStatus ST);
        public Task<AcceptedComplaint> AcceptedComplaint(int Id, AcceptedComplaint ST);
        public Task<List<ComplaintDTO>> GetComplaintByDate(DateTime From, DateTime To);
        public Task<GetComplaintDTO> GetComplaintById(int Id);
        public Task<List<(GetComplaintDTO, int)>> ComplaintClosedinDpartment(int Id);
        public Task<int> AllClosedComplaint();
        public Task<NotMySpecialistComplaint> ComplaintNotMySpecialist(int Id, NotMySpecialistComplaint ST);
        public Task<List<GetAllRejectedDTO>> AllRejectedComplaint();

        public Task<ComplaintDTO> EditDepartment(int ComplaintId, int departmentId);


        public Task<List<GetComplaintDTO>> GetAllComplaintByDepartment(int departmentTypeId);
        public Task<GetComplaintDTO> GetComplaintByIdAndDepartment(int complaintId, int departmentTypeId);

    }
}
