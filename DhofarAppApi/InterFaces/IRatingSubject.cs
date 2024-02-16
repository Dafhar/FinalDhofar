using DhofarAppApi.Model;

namespace DhofarAppApi.InterFaces
{
    public interface IRatingSubject
    {
        public Task<List<RatingSubject>> GetAll();
    }
}
