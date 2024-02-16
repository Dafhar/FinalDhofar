namespace DhofarAppWeb.Model
{
    public class Poll
    {
       
            public int Id { get; set; }

            public int SubjectId { get; set; }

            public required string Question { get; set; }

            public List<PollOption> Options { get; set; }

            public Subject Subject { get; set; }
            public List<UserVote> UserVotes { get; set; }

     }
}
