using DhofarAppApi.Dtos.Comment;
using DhofarAppApi.Dtos.ComplaintFiles;
using DhofarAppApi.Dtos.SubjectFiles;
using DhofarAppApi.Model;

namespace DhofarAppApi.Dtos.Subject
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
