namespace DhofarAppWeb.Model
{
    public class UserColors
    {
        public int ColorsId { get; set; }

        public string UserId { get; set; }

        public Colors Colors { get; set; }

        public User User { get; set; }
    }
}
