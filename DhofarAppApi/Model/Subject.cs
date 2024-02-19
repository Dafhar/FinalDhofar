namespace DhofarAppApi.Model
{
    public class Subject
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int GeneralSubjectsTypesId { get; set; }

        public string PrimarySubject { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int LikeCounter { get; set; }

        public int DisLikeCounter { get; set; }

        public int VisitorCounter { get; set; } = 0;

        public DateTime CreatedTime { get; set; }

        //public int SubjectTypeId { get; set; }  

        // Navigations 

        public List<SubjectFiles>? Files { get; set; }

        public List<RatingSubject>? RatingSubjects { get; set; }

        public List<CommentSubject>? CommentSubjects { get; set; }

        public List<FavoriteSubject>? FavoriteSubjects { get; set; }

        public GeneralSubjectsType GeneralSubjectsTypes { get; set; }

        public List<SubjectTypeSubject> SubjectTypeSubjects { get; set; }

        public User User { get; set; }
        public Poll? Poll { get; set; }


    }
}
