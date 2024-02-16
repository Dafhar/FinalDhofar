namespace DhofarAppApi.Dtos
{
    public class UserSubnetToken
    {
        public string Token { get; set; }
    }

    public class UserSubnetRegister
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserProfile
    {

    }
}
