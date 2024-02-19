using DhofarAppApi.Dtos.ReplyComment;
using DhofarAppApi.Model;

namespace DhofarAppApi.Dtos.Comment
{
    public class GetSubjectCommentDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public int SubjectId { get; set; }

        public string Comment { get; set; }

        public DateTime CommentTime { get; set; }

        public int UpVoteCounter { get; set; }

        public int DownVoteCounter { get; set; }

        public string File { get; set; }

        public string UserImageUrl { get; set; }

        public List<GetReplyCommentDTO> ReplyComments { get; set; }

        public List<DhofarAppApi.Model.User> Users { get; set; }
    }
}
