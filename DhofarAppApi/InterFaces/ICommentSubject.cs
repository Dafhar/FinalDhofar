using DhofarAppApi.Dtos.Comment;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DhofarAppApi.InterFaces
{
    public interface ICommentSubject
    {
        public Task<GetSubjectCommentDTO> CreateCommentSubject(PostCommentDTO postComment,int subjectId );

        public Task<List<GetSubjectCommentDTO>> GetAllCommentForSubjectId(int subjectId);

        public Task<GetSubjectCommentDTO> EditComment(int  commentId, PostCommentDTO commentDTO);

        public Task DeleteComment(int commentId);

        public Task VoteUpComment(int commentId);

        public Task RemoveVoteUpComment(int commentId);

        public Task VoteDownComment(int commentId);

        public Task RemoveVoteDownComment(int commentId);

    }
}
