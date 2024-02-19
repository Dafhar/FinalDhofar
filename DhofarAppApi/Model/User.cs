using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace DhofarAppApi.Model
{
    public class User : IdentityUser
    {
        
        public int IdentityNumber { get; set; }

        public int? Likes { get; set; }

        public string  FullName { get; set; }
        public string?  MyColor { get; set; }


        public string? SelectedLanguage { get; set; }
        public string? Country { get; set; }

        public string? ImageURL { get; set; }

        public string CodeNumber { get; set; }

        public string? Gender { get; set; }

        public string? Description { get; set; }

        public DateTime JoinedDate { get; set; }

        public DateTime LogInDate { get; set; }

        public int? NumberOfVisitors { get; set; }



        public List<Complaint>? Complaints { get; set; }

        public List<Subject>? Subjects { get; set; }

        public List<FavoriteSubject>? FavoriteSubjects { get; set; }

        public List<CommentSubject>? CommentSubjects { get; set; }

        public List<DeviceToken>? DeviceTokens { get; set; }

        public List<RatingSubject>? RatingSubjects { get; set; }

        public bool Sound { get; set; } = true;

        public List<UserCommentVote>? userCommentVotes { get; set; }

        public List<CommentReplies>? CommentReplies { get; set; }

        public List<UserVote>? UserVotes { get; set; }

        public List<DepartmentAdmin>? DepartmentAdmins { get; set; }

        

    }

}
