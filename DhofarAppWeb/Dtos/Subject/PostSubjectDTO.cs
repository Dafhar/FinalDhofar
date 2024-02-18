using DhofarAppWeb.Dtos.ComplaintFiles;
using DhofarAppWeb.Dtos.SubjectFiles;

namespace DhofarAppWeb.Dtos.Subject
{
    public class PostSubjectDTO
    {
        public string UserId { get; set; }

        public int GeneralSubjectsTypesId { get; set; }


        public  string PrimarySubject { get; set; }

        public int SubjectTypeId { get; set; }

        public  string Title { get; set; }


        public  string Description { get; set; }

        public string? PollQuestion { get; set; }

        public List<string>? PollOptions { get; set; }
        public List<string>? ImageUrl { get; set; }


    }
}
