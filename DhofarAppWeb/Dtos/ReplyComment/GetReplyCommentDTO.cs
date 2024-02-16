namespace DhofarAppWeb.Dtos.ReplyComment
{
    public class GetReplyCommentDTO
    {
        public int Id { get; set; }

        public int CommentSubjectId { get; set; }
        public string file { get; set; }
        public  string UserName { get; set; }

        public string ReplyComment { get; set; }


    }
}
