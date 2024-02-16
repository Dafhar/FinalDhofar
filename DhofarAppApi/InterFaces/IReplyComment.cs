using DhofarAppApi.Dtos.ReplyComment;
using DhofarAppApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DhofarAppApi.InterFaces
{
    public interface IReplyComment
    {
        public Task<GetReplyCommentDTO> AddReply(PostReplyCommentDTO commentReplies, int commentSubjectId);

        public Task<List<GetReplyCommentDTO>> GetCommentReplies(int commentSubjectId);

        public Task<GetReplyCommentDTO> GetCommentRepliesById(int replyId);

        public Task<CommentReplies> DeleteReplyComment(int replyId);

        public Task<GetReplyCommentDTO> EditReplyComment(PostReplyCommentDTO commentReplies, int replyId);


    }
}
