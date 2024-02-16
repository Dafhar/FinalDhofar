using DhofarAppApi.Model;

namespace DhofarAppApi.InterFaces
{
    public interface IOnBoardScreen
    {
        Task<List<OnBoardScreen>> GetAll();

    }
}
