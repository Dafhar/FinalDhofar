namespace DhofarAppApi.Dtos.User
{
    public class GetUserDTO
    {
        public string Id { get; set; }

        public int IdentityNumber { get; set; }

        public string UserName { get; set; }

        public IList<string> Roles { get; set; }


    }
}
