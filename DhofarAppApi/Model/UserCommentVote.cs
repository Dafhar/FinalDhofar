namespace DhofarAppApi.Model
{
    public class UserCommentVote
    {
        public int Id { get; set; }

        public int CommentSubjectId { get; set; }

        public string UserId { get; set; }

        public bool VoteUp { get; set; }

        public bool  VoteDown { get; set; }

        public CommentSubject CommentSubject { get; set; }

        public User User { get; set; }
    }
}
