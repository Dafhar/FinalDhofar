using DhofarAppApi.Model;

namespace DhofarAppApi.Dtos.Subject
{
    public class PollDTO
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }

        public required string Question { get; set; }

        public List<PollOptionDTO> Options { get; set; }
    }
    public class PollOptionDTO
    {
        public int Id { get; set; }

        public int PollId { get; set; }

        public required string OptionText { get; set; }

        public int VoteCount { get; set; }
    }
}
