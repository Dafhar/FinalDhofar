using DhofarAppApi.Dtos.Main;
using DhofarAppApi.Model;

namespace DhofarAppApi.InterFaces
{
    public interface IMain
    {
        public Task<SubjectForMainDTO> GetAll();
    }
}
