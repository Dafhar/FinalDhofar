namespace DhofarAppWeb.Model
{
    public class CommentReplies
    {
        public int Id { get; set; }

        public required string ReplyComment { get; set; }

        public int CommentSubjectId { get; set; }

        public required string UserId { get; set; }

        public CommentSubject CommentSubject { get; set; }

        public User User { get; set; }

        public string  File { get; set; }
    }
}
