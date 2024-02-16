namespace DhofarAppApi.InterFaces
{
    public interface IUploadFile
    {
        public Task<string> uploadfile(IFormFile file);
    }
}
