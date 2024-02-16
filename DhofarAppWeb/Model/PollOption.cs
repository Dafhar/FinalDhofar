namespace DhofarAppWeb.Model
{
    public class PollOption
    {
        public int Id { get; set; }

        public int PollId { get; set; }

        public required string OptionText { get; set; }

        public int VoteCount { get; set; }

        public Poll Poll { get; set; }

        public List<UserVote> UserVotes { get; set; }
    }
}
