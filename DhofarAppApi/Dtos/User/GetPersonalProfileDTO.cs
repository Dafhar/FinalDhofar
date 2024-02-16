namespace DhofarAppApi.Dtos.User
{
    public class GetPersonalProfileDTO
    {
        public int SubjectCounter { get; set; }

        public int CommentCounter { get; set; }

        public double Rate { get; set; }

        public string FullName { get; set; }
        public string Description { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string Color { get; set; }

        public string Country { get; set; }
        public string Code { get; set; }

        public string Gender { get; set; }

        public string ImageUrl { get; set; }
    }
}
