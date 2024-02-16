using DhofarAppApi.Dtos.Subject;
using DhofarAppApi.Model;
using System.Security.Claims;

namespace DhofarAppApi.InterFaces
{
    public interface ISubject
    {

        public Task<GetSubjectDTO> CreateSubject(PostSubjectDTO postSubjectDto);

        public Task<List<GetSubjectDTO>> GetAllSubjects();

        public Task<GetSubjectDTO> GetSubjectById(int Id);

        public Task<List<GetSubjectDTO>> GetSubjectByUserId();

        public Task<Subject> DeleteSubject(int Id);

        public Task<GetSubjectDTO> EditSubject(int subjectId, EditSubjectDTO postSubjectDto);

        public void IncrementVisitorCounter(int subjectId);

        public int getVisitorCounter(int subjectId);
        public Task<string> Like(int subjectId);
        public Task<string> Dislike(int subjectId);
        public Task AddSubjectToFavorite(int subjectId);

        public Task RemoveSubjectFromFavorite(int subjectId);
        public Task<List<GetSubjectDTO>> GetFavoriteSubjects();


        public Task<bool> VoteForPollOption(int subjectId, int PollId, int pollOptionId);

        public Task<List<PollDTO>> GetPollBySubject(int subjectId);

        public Task<bool> DeleteVote(int subjectId, int PollId, int pollOptionId);

        public Task<Subject> GetTheMostSubjectInteract();


        Task<int> GetCountOfSubjects();




    }
}
