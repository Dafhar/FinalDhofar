using DhofarAppApi.Model;

namespace DhofarAppApi.Dtos.ReplyComment
{
    public class GetReplyCommentDTO
    {
        public int Id { get; set; }

        public int CommentSubjectId { get; set; }

        public string file { get; set; }

        public  string UserName { get; set; }

        public string UserImageUrl { get; set; }

        public string ReplyComment { get; set; }

    }
    public class GetAllReplyCommentDTO
    {
        public int Id { get; set; }

        public int CommentSubjectId { get; set; }

        public string file { get; set; }

        public string FullName { get; set; }

        public string UserImageUrl { get; set; }

        public string ReplyComment { get; set; }

        public string Role { get; set; }
    }
    
}
