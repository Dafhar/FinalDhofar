using DhofarAppApi.Model;

namespace DhofarAppApi.InterFaces
{
    public interface IVisitor
    {
        Task<string> ContinueAsVisitor();

        Task<int> CountVisitors();

        Task<(List<object>, int)> GetVisitorStatistics();

    }
}
