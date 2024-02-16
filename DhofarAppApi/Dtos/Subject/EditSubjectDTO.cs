namespace DhofarAppApi.Dtos.Subject
{
    public class EditSubjectDTO
    {
        public  string PrimarySubject { get; set; }


        public  string Title { get; set; }

        public  string Description { get; set; }

        public string? PollQuestion { get; set; }

        public List<string>? PollOptions { get; set; }
        public List<string>? ImagUrl { get; set; }

    }
}
