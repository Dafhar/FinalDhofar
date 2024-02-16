using DhofarAppApi.Data;
using DhofarAppApi.Dtos.Complaint;
using DhofarAppApi.Dtos.ComplaintFiles;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.EntityFrameworkCore;
using Image = SixLabors.ImageSharp.Image;
using System.Security.Claims;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using SendGrid.Helpers.Mail;

namespace DhofarAppApi.Services
{
    public class ComplaintServices : IComplaint
    {
        private readonly AppDbContext _db;
        private readonly INotification _notification;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenService;



        public ComplaintServices(AppDbContext db, INotification notification, IHttpContextAccessor httpContextAccessor, JWTTokenServices jWTTokenServices)
        {
            _db = db;
            _notification = notification;
            _httpContextAccessor = httpContextAccessor;
            _jWTTokenService = jWTTokenServices;
        }


        public async Task<ComplaintDTO> CreateComplaint(PostComplaintDTO complaint)
        {
            var now = DateTime.UtcNow;
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);
            // Create a new Complaint entity
            var newComplaint = new Complaint
            {
                Title = complaint.Title,
                Description = complaint.Description,
                CategoryId = complaint.CategoryId,
                DepartmentTypeId = complaint.DepartmentTypeId,
                Time = now,
                IsAccepted = false,
                Location = complaint.Location,
                UserId = userId,
                State = complaint.State,
                Status_En = "New",
                Status_Ar = "جديدة",
                Files = new List<ComplaintsFile>() // Initialize Files property with an empty list
            };

            // Add complaint to database
            await _db.Complaints.AddAsync(newComplaint);
            await _db.SaveChangesAsync();

            // Process and save files sequentially
            foreach (var file in complaint.files)
            {
                if (file != null && file.Length > 0)
                {
                    
                    // Create a new ComplaintsFile entity for each file
                    var newComplaintFile = new ComplaintsFile
                    {
                        FilePaths =file ,
                        ComplaintId = newComplaint.Id // Assign ComplaintId
                    };

                    // Add ComplaintsFile to database
                    await _db.ComplaintsFiles.AddAsync(newComplaintFile);
                }
            }

            // Save changes to the database
            await _db.SaveChangesAsync();

            // Prepare and return response DTO
            var getComplaintdto = new ComplaintDTO
            {
                UserName=user.UserName,
                Description = newComplaint.Description,
                State = newComplaint.State,
                Title = newComplaint.Title,
                CategoryID = newComplaint.CategoryId,
                Status_En = newComplaint.Status_En,
                Status_Ar = newComplaint.Status_Ar,
                Location = newComplaint.Location,
                DepartmentTypeId = newComplaint.DepartmentTypeId,
                Files = newComplaint.Files.Select(fi => new GetComplaintFilesDTO
                {
                   FilePaths = fi.FilePaths,
                    ComplaintId = fi.ComplaintId
                }).ToList()
            };

            return getComplaintdto;
        }

        
    }
}
