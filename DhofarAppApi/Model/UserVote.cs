namespace DhofarAppApi.Model
{
    public class UserVote
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int PollId { get; set; }

        public int PollOptionId { get; set; }

        public User User { get; set; }

        public Poll Poll { get; set; }

        public PollOption PollOption { get; set; }
    }
}
