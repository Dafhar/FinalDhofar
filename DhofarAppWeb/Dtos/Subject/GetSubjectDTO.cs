using DhofarAppWeb.Dtos.Comment;
using DhofarAppWeb.Dtos.ComplaintFiles;
using DhofarAppWeb.Dtos.SubjectFiles;
using DhofarAppWeb.Model;

namespace DhofarAppWeb.Dtos.Subject
{
    public class GetSubjectDTO
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? PrimarySubject { get; set; }
                     
        public int? SubjectTypeId { get; set; }
                   
        public string? Title { get; set; }
        public int LikeCounter { get; set; }

        public int DisLikeCounter { get; set; }


        public string? Description { get; set; }

        public int VisitorCounter { get; set; }
        public PollDTO? Poll { get; set; }

        public List<GetSubjectFilesDTO>? Files { get; set; }


        public List<GetSubjectCommentDTO>? CommentsSubjects { get; set; }
    }
}
