namespace DhofarAppApi.Model
{
    public class CommentSubject
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }

        public string UserId { get; set; }

        public string Comment { get; set; }

        public string File { get;set; }

        public DateTime CommentTime { get; set; }

        public int UpVoteCounter { get; set; }

        public int DownVoteCounter { get; set; }    

        public Subject Subject { get; set; }    

        public User User { get; set; }

        public  List<UserCommentVote> UserCommentVotes { get; set; }

        public List<CommentReplies>? CommentReplies { get; set; }

        

    }
}
