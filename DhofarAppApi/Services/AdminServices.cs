using DhofarAppApi.Data;
using DhofarAppApi.Dtos.Category;
using DhofarAppApi.Dtos.Complaint;
using DhofarAppApi.Dtos.ComplaintFiles;
using DhofarAppApi.Dtos.SubCategory;
using DhofarAppApi.Dtos.User;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System.Resources;
using System.Security.Claims;

namespace DhofarAppApi.Services
{
    public class AdminServices : IAdmin
    {

        private readonly AppDbContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JWTTokenServices _jWTTokenService;
        private readonly IConfiguration _configuration;
        private readonly ResourceManager _resourceManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AdminServices(AppDbContext db, SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor , JWTTokenServices jWTTokenService)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _jWTTokenService = jWTTokenService;
            _resourceManager = new ResourceManager("DhofarAppApi.Resources.ErrorMessages", typeof(IdentityUserServices).Assembly);
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<UserDTO> RegisterEmployee([FromBody] RegisterEmployeeDTO model, ModelStateDictionary modelState, ClaimsPrincipal principal)
        {

            var lastIdentityNumber = await _db.IdentityNumbers.FirstOrDefaultAsync();
            int generateIdentityId = (lastIdentityNumber != null) ? lastIdentityNumber.Value : 1;

            var user = new User()
            {
                UserName = model.UserName,
                FullName = model.Fullname,
                Email = model.Email,
                CodeNumber = model.CodeNumber,
                PhoneNumber = model.PhoneNumber,
                IdentityNumber = generateIdentityId++,
                JoinedDate = DateTime.UtcNow

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
                IList<string> role = new List<string>() { "Admin" };
                await _userManager.AddToRolesAsync(user, role);

                if (lastIdentityNumber != null)
                {
                    lastIdentityNumber.Value = generateIdentityId;
                }
                else
                {
                    await _db.IdentityNumbers.AddAsync(new IdentityNumber { Value = generateIdentityId });
                }

                //var usertoken = new DeviceToken()
                //{
                //    UserId = user.Id,
                //    // Token = DeviceToken from mobile
                //};

                var departmentAdmin = new DepartmentAdmin
                {
                    UserId = user.Id,
                    DepartmentTypeId = model.DepatmentTypeId
                };

                await _db.DepartmentAdmins.AddAsync(departmentAdmin);

                //await _context.deviceTokens.AddAsync(usertoken);

                await _db.SaveChangesAsync();

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
                if (user.SelectedLanguage == "ar")
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
            }
            return null;
        }
        public async Task<UserDTO> RegisterSuperAdmin([FromBody] RegisterSuperAdminDTO model, ModelStateDictionary modelState, ClaimsPrincipal principal)
        {
            var lastIdentityNumber = await _db.IdentityNumbers.FirstOrDefaultAsync();
            int generateIdentityId = (lastIdentityNumber != null) ? lastIdentityNumber.Value : 1;

            var user = new User()
            {
                UserName = model.UserName,
                FullName = model.Fullname,
                Email = model.Email,
                CodeNumber = model.CodeNumber,
                PhoneNumber = model.PhoneNumber,
                IdentityNumber = generateIdentityId++,
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
                IList<string> role = new List<string>() { "Super Admin" };
                await _userManager.AddToRolesAsync(user, role);

                if (lastIdentityNumber != null)
                {
                    lastIdentityNumber.Value = generateIdentityId;

                }
                else
                {
                    await _db.IdentityNumbers.AddAsync(new IdentityNumber { Value = generateIdentityId });
                }

                //var usertoken = new DeviceToken()
                //{
                //    UserId = user.Id,
                //    // Token = DeviceToken from mobile
                //};

                //await _context.deviceTokens.AddAsync(usertoken);


                await _db.SaveChangesAsync();

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
                if (user.SelectedLanguage == "ar")
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
            }
            return null;
        }

        public async Task<List<(UserDTO User, int SubjectCount, int CommentCount)>> GetTopFiveCommenters()
        {
            var topFiveUsers = await _db.Users
                .OrderByDescending(u => u.Subjects.Count)
                .Take(5)
                .Select(u => new
                {
                    User = new UserDTO
                    {
                        UserName = u.UserName,
                        // You can include other properties from UserDTO if needed
                    },
                    SubjectCount = u.Subjects.Count,
                    CommentCount = u.CommentSubjects.Count
                })
                .ToListAsync();

            return topFiveUsers
                .Select(u => (u.User, u.SubjectCount, u.CommentCount))
                .ToList();
        }




        public async Task<(List<object>, int)> GetUserStaticsPerDay()
        {
            var visitorsPerDay = await _db.Users
           .GroupBy(v => v.JoinedDate.Date)
           .Select(g => new { Date = g.Key, Count = g.Count() })
           .ToListAsync();

            // Example: Get total number of visitors
            var userCount = await _db.Users.CountAsync();

            return (visitorsPerDay.Cast<object>().ToList(), userCount);
        }



        public async Task<OperationResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return OperationResult.Failed("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
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



        public async Task<UserDTO> ChangeUserRole(string userId, string oldRole, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var removeResult = await _userManager.RemoveFromRolesAsync(user, new List<string>() { oldRole });
            if (!removeResult.Succeeded)
            {
                return null;
            }

            var addRole = await _userManager.AddToRolesAsync(user, new List<string>() { newRole });
            if (!addRole.Succeeded)
            {
                return null;
            }
            return new UserDTO
            {
                Id = userId,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user)
            };

        }


        public async Task<GetUserDTO> GetUser(string userName)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return null;
            }

            var userDTO = new GetUserDTO
            {
                Id = user.Id,
                IdentityNumber = user.IdentityNumber,
                UserName = user.UserName,
                Roles = await _userManager.GetRolesAsync(user),
            };

            return userDTO;
        }

        public async Task<Dictionary<string, int>> GetUserStaticsPerMonth()
        {
            var loginStatistics = await _db.Users
                .Where(login => login.LogInDate != null)
                .GroupBy(login => login.LogInDate.ToString("yyyy-MM"))
                .Select(group => new { Month = group.Key, Count = group.Count() })
                .ToDictionaryAsync(item => item.Month, item => item.Count);

            return loginStatistics;
        }

        public async Task<int> GetUsersCount()
        {
            return await _db.Users.CountAsync();
        }

        public async Task<List<GetUserDTO>> GetAllUser()
        {
            var users = await _db.Users.ToListAsync();
            var userDTOs = new List<GetUserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userDTO = new GetUserDTO
                {
                    Id = user.Id,
                    IdentityNumber = user.IdentityNumber,
                    UserName = user.UserName,
                    Roles = roles,
                };

                userDTOs.Add(userDTO);
            }

            return userDTOs;
        }


        public async Task<CreatCategortDTO> CreateCategort(CreatCategortDTO category)
        {
            Category newCategory = new Category()
            {
                Name_En = category.Name_En,
                Name_Ar = category.Name_Ar,


                subcategories = category.subcategories.Select(cate => new SubCategory
                {
                    Name_En = cate.Name_En,
                    Name_Ar = cate.Name_Ar,

                }).ToList()
            };
            await _db.Categories.AddAsync(newCategory);
            await _db.SaveChangesAsync();

            var categorydto = new CreatCategortDTO
            {
                Name_En = newCategory.Name_En,
                Name_Ar = newCategory.Name_Ar,
                subcategories = newCategory.subcategories.Select(cate => new CreateSubCategoryDTO
                {
                    Name_En = cate.Name_En,
                    Name_Ar = cate.Name_Ar,

                }).ToList()
            };

            return categorydto;
        }


        public async Task DeleteCategort(int Id)
        {
            var deletedCategory = await _db.Categories.FindAsync(Id);
            if (deletedCategory != null)
            {
                _db.Entry(deletedCategory).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
            }
        }
        public async Task<UpdateCategoryDTO> UpdateCategory(int Id, UpdateCategoryDTO updatecategoryDto)
        {
            var category = await _db.Categories.FindAsync(Id);

            if (category == null)
            {
                return null;
            }

            category.Name_En = updatecategoryDto.Name_En;
            category.Name_Ar = updatecategoryDto.Name_Ar;

            _db.Entry(category).State = EntityState.Modified;

            await _db.SaveChangesAsync();

            var updatedCategoryDTO = new UpdateCategoryDTO
            {
                Name_En = category.Name_En,
                Name_Ar = category.Name_Ar,
            };

            return updatedCategoryDTO;
        }



        public async Task<CreateSubCategoryDTO> CreateSubcategory(int categoryId, CreateSubCategoryDTO subCategory)
        {
            var category = await _db.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return null;
            }

            var newSubcategory = new SubCategory
            {
                Name_En = subCategory.Name_En,
                Name_Ar = subCategory.Name_Ar,
                CategoryId = categoryId
            };

            try
            {
                await _db.SubCategories.AddAsync(newSubcategory);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return null;
            }

            var createdSubcategoryDTO = new CreateSubCategoryDTO
            {
                Name_En = newSubcategory.Name_En,
                Name_Ar = newSubcategory.Name_Ar,
            };

            return createdSubcategoryDTO;
        }

        public async Task<CreateSubCategoryDTO> UpdateSubcategory(int categoryId, int subCategoryId, CreateSubCategoryDTO subCategory)
        {
            var existingSubcategory = await _db.SubCategories.FindAsync(subCategoryId);
            if (existingSubcategory == null)
                return null;

            // Ensure that the existing subcategory belongs to the specified category
            if (existingSubcategory.CategoryId != categoryId)
                return null;

            existingSubcategory.Name_En = subCategory.Name_En;
            existingSubcategory.Name_Ar = subCategory.Name_Ar;

            _db.Entry(existingSubcategory).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            var updatedSubcategoryDTO = new CreateSubCategoryDTO
            {
                Name_En = existingSubcategory.Name_En,
                Name_Ar = existingSubcategory.Name_Ar,
            };

            return updatedSubcategoryDTO;
        }

        public async Task DeleteSubcategory(int categoryId, int subCategoryId)
        {
            var subCategory = await _db.SubCategories.FindAsync(subCategoryId);
            if (subCategory != null && subCategory.CategoryId == categoryId)
            {
                _db.SubCategories.Remove(subCategory);
                await _db.SaveChangesAsync();
            }
        }



        public async Task<List<GetComplaintDTO>> GetAllUserComplaint()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);

            return await _db.Complaints
                .Where(c=> c.IsAccepted == true)
                .Select(getComplaintdto => new GetComplaintDTO
                 {
                Description = getComplaintdto.Description,
                Location = getComplaintdto.Location,
                State = getComplaintdto.State,
                Title = getComplaintdto.Title,
                Status = user.SelectedLanguage == "ar" ? getComplaintdto.Status_Ar : getComplaintdto.Status_En,
                DepartmentTypeId = getComplaintdto.DepartmentTypeId,
                Time = getComplaintdto.Time,
                Files = getComplaintdto.Files.Select(file => new GetComplaintFilesDTO
                {
                    Id = file.Id,
                    FilePaths = file.FilePaths,
                    ComplaintId = file.ComplaintId,

                }).ToList()
            }).ToListAsync();
        }



        public async Task<string> DeleteComplaint(int Id, string why)
        {
            var deletedComplaint = await _db.Complaints.FindAsync(Id);
            var user = await _db.Users.FindAsync(deletedComplaint.UserId);
            var token = await _db.deviceTokens.FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (deletedComplaint != null)
            {
                _db.Entry(deletedComplaint).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                //await _notification.SentToUser("deleted your complaint", why, token.Token);

                return "Deleted successfully";
            }
            return "Not Found";

        }

        public async Task<EditComplaintStatus> EditComplaintStatus(int Id, EditComplaintStatus ST)
        {
            var EditedComplaint = await _db.Complaints.FindAsync(Id);
            //  var token = await _db.deviceTokens.FirstOrDefaultAsync(u=>u.UserId == EditedComplaint.UserId);
            if (EditedComplaint != null)
            {
                EditedComplaint.Status_En = ST.Status_En;
                EditedComplaint.Status_Ar = ST.Status_Ar;
                _db.Entry(EditedComplaint).State = EntityState.Modified;
                //   await _notification.SentToUser("Status", $"your compliant status changed to {ST} ", token.Token);

                await _db.SaveChangesAsync();
                var chandespecial = new EditComplaintStatus
                {
                    Status_En = EditedComplaint.Status_En,
                    Status_Ar = EditedComplaint.Status_Ar,
                };
                return chandespecial;

            }
            return null;

        }


        public async Task<List<ComplaintDTO>> GetComplaintByDate(DateTime From, DateTime To)
        {
            var complaints = await _db.Complaints
                .Where(x => x.Time >= From && x.Time <= To)
                .ToListAsync();

            var compliantDTOs = complaints.Select(complaint => new ComplaintDTO
            {
                Description = complaint.Description,
                Location = complaint.Location,
                State = complaint.State,
                Time = complaint.Time,
                Title = complaint.Title,
                DepartmentTypeId = complaint.DepartmentTypeId,
                Status_En = complaint.Status_En,
                Status_Ar = complaint.Status_Ar
            }).ToList();

            return compliantDTOs;
        }


    

        public async Task<GetComplaintDTO> GetComplaintById(int Id)
        {
            var lang = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString().Split(',')[0].Trim();

            var complaint = await _db.Complaints
                .Include(c => c.Files)
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (complaint == null)
            {
                return null; // Or handle the case where the complaint with the given ID is not found
            }

            var complaintDTO = new GetComplaintDTO
            {
                Description = complaint.Description,
                Location = complaint.Location,
                Status = lang == "en" ? complaint.Status_En : complaint.Status_Ar,
                Title = complaint.Title,
                State = complaint.State,
                DepartmentTypeId = complaint.DepartmentTypeId,
                Time = complaint.Time,
                Files = complaint.Files.Select(file => new GetComplaintFilesDTO
                {
                    Id = file.Id,
                    FilePaths = file.FilePaths,
                    ComplaintId = file.ComplaintId
                }).ToList()
            };

            return complaintDTO;
        }



        public async Task<List<(GetComplaintDTO, int)>> ComplaintClosedinDpartment(int Id)
        {
            var closedComplaintsByDepartment = await _db.Complaints
                .Where(com => com.Status_En == "Closed" || com.Status_Ar == "مغلقة")
                .GroupBy(com => com.DepartmentTypeId)
                .Select(group => new { DepartmentTypeId = group.Key, Count = group.Count(), Complaints = group.ToList() })
                .ToListAsync();

            var result = closedComplaintsByDepartment
                .SelectMany(item => item.Complaints
                    .Select(complaint =>
                    {
                        var getComplaintDTO = new GetComplaintDTO
                        {
                            Description = complaint.Description,
                            Location = complaint.Location,
                            Status = complaint.Status_En,
                            Title = complaint.Title,
                            State = complaint.State,
                            DepartmentTypeId = complaint.DepartmentTypeId,
                        };
                        return (getComplaintDTO, item.Count);
                    }))
                .ToList();

            return result;
        }

        public async Task<ComplaintDTO> EditDepartment(int ComplaintId, int departmentId)
        {
            var EditedComplaint = await _db.Complaints.FindAsync(ComplaintId);

            if (EditedComplaint != null)
            {
                EditedComplaint.DepartmentTypeId = departmentId;
                EditedComplaint.MySpecialist = true;
                await _db.SaveChangesAsync();
            }

            var edited = new ComplaintDTO
            {
                DepartmentTypeId = EditedComplaint.DepartmentTypeId,
                CategoryID = EditedComplaint.CategoryId,
                Description = EditedComplaint.Description,
                Location = EditedComplaint.Location,
                State = EditedComplaint.State,
                Status_Ar = EditedComplaint.Status_Ar,
                Status_En = EditedComplaint.Status_En,
                Title = EditedComplaint.Title,
                Time = EditedComplaint.Time,
                Files = EditedComplaint.Files != null
                 ? EditedComplaint.Files.Select(file => new GetComplaintFilesDTO
                 {
                     Id = file.Id,
                     FilePaths = file.FilePaths,
                     ComplaintId = file.ComplaintId
                 }).ToList()
                   : new List<GetComplaintFilesDTO>()
            };

            return edited;
        }

        public async Task<List<GetComplaintDTO>> GetAllComplaintByDepartment(int departmentTypeId)
        {
            var lang = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString().Split(',')[0].Trim();

            return await _db.Complaints
                .Where(c => c.DepartmentTypeId == departmentTypeId)
                .Select(getComplaintdto => new GetComplaintDTO
                {
                    Description = getComplaintdto.Description,
                    Location = getComplaintdto.Location,
                    State = getComplaintdto.State,
                    Title = getComplaintdto.Title,
                    Status = lang == "en" ? getComplaintdto.Status_En : getComplaintdto.Status_Ar,
                    DepartmentTypeId = getComplaintdto.DepartmentTypeId,
                    Time = getComplaintdto.Time,
                    Files = getComplaintdto.Files.Select(file => new GetComplaintFilesDTO
                    {
                        Id = file.Id,
                        FilePaths = file.FilePaths,
                        ComplaintId = file.ComplaintId
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<GetComplaintDTO> GetComplaintByIdAndDepartment(int complaintId, int departmentTypeId)
        {
            var lang = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString().Split(',')[0].Trim();

            return await _db.Complaints
                .Where(c => c.Id == complaintId && c.DepartmentTypeId == departmentTypeId)
                .Select(getComplaintdto => new GetComplaintDTO
                {
                    Description = getComplaintdto.Description,
                    Location = getComplaintdto.Location,
                    State = getComplaintdto.State,
                    Title = getComplaintdto.Title,
                    Status = lang == "en" ? getComplaintdto.Status_En : getComplaintdto.Status_Ar,
                    DepartmentTypeId = getComplaintdto.DepartmentTypeId,
                    Time = getComplaintdto.Time,
                    Files = getComplaintdto.Files.Select(file => new GetComplaintFilesDTO
                    {
                        Id = file.Id,
                        FilePaths = file.FilePaths,
                        ComplaintId = file.ComplaintId
                    }).ToList()
                }).FirstOrDefaultAsync();
        }


        public async Task<NotMySpecialistComplaint> ComplaintNotMySpecialist(int Id, NotMySpecialistComplaint ST)
        {
            var EditedComplaint = await _db.Complaints.FindAsync(Id);
            if (EditedComplaint != null)
            {
                EditedComplaint.MySpecialist = ST.NotMySpecialist;
                await _db.SaveChangesAsync();
                var chandespecial = new NotMySpecialistComplaint
                {
                    NotMySpecialist = EditedComplaint.MySpecialist,
                };
                return chandespecial;
            }

            return null;
        }


        public async Task<int> AllClosedComplaint()
        {
            return await _db.Complaints.Where(com => com.Status_En == "Closed" || com.Status_Ar == "مغلقة").CountAsync();
        }


        public async Task<List<GetAllRejectedDTO>> AllRejectedComplaint()
        {
            var rejectedComplaints = await _db.Complaints
                .Where(com => com.MySpecialist == false)
                .Select(com => new GetAllRejectedDTO
                {
                    Description = com.Description,
                    Location = com.Location,
                    State = com.State,
                    DepartmentTypeId = com.DepartmentTypeId,

                })
                .ToListAsync();

            return rejectedComplaints;
        }


        public async Task<AcceptedComplaint> AcceptedComplaint(int Id, AcceptedComplaint ST)
        {
            var EditedComplaint = await _db.Complaints.FindAsync(Id);
            if (EditedComplaint != null)
            {
                EditedComplaint.IsAccepted = ST.IsAccepted;
                _db.Entry(EditedComplaint).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                var chandeaccept = new AcceptedComplaint
                {
                    IsAccepted = EditedComplaint.IsAccepted,
                };
                return chandeaccept;
            }

            return null;
        }


        public async Task<List<GetAllComplaintAdmin>> GetAllComplaint()
        {
            return await _db.Complaints.Select(getComplaintdto => new GetAllComplaintAdmin
            {
                Description = getComplaintdto.Description,
                Location = getComplaintdto.Location,
                Status_Ar=getComplaintdto.Status_Ar,
                Status_En=getComplaintdto.Status_En,
                Title = getComplaintdto.Title,
                DepartmentTypeId = getComplaintdto.DepartmentTypeId,
                Time = getComplaintdto.Time,
                Files = getComplaintdto.Files.Select(file => new GetComplaintFilesDTO
                {
                    Id = file.Id,
                    FilePaths = file.FilePaths,
                    ComplaintId = file.ComplaintId,

                }).ToList()
            }).ToListAsync();

        }



    }
}
