using DhofarAppApi.Data;
using DhofarAppApi.Dtos.ReplyComment;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DhofarAppApi.Services
{
    public class CommentRepliesServices : IReplyComment
    {
        private readonly AppDbContext _context;
        private readonly INotification _notification;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenService;


        public CommentRepliesServices(AppDbContext context, INotification notification, IHttpContextAccessor httpContextAccessor, JWTTokenServices jWTTokenServices)
        {
            _context = context;
            _notification = notification;
            _httpContextAccessor = httpContextAccessor;
            _jWTTokenService = jWTTokenServices;
        }

        public async Task<GetReplyCommentDTO> AddReply(PostReplyCommentDTO commentReplies, int commentSubjectId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _context.Users.FindAsync(userId);
            if (userId == null)
            {
                return null;
            }
            else
            {
                var commentReply = new CommentReplies
                {
                    CommentSubjectId = commentSubjectId,
                    UserId = userId,
                    ReplyComment = commentReplies.ReplyComment,
                    File=commentReplies.imgUrl,

                };
               
                
                

                await _context.CommentReplies.AddAsync(commentReply);
                await _context.SaveChangesAsync();

                var commentreplay = new GetReplyCommentDTO
                {
                    UserName= user.UserName,
                    ReplyComment = commentReply.ReplyComment,
                    file= commentReply.File,
                    CommentSubjectId= commentReply.CommentSubjectId,
                    Id= commentReply.Id,
                };

                return commentreplay;
            }
        }

        public async Task<CommentReplies> DeleteReplyComment(int replyId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            if (userId == null)
            {
                return null;
            }

            var replyComment = await _context.CommentReplies.FirstOrDefaultAsync(rc => rc.Id == replyId && rc.UserId == userId);

            if (replyComment != null)
            {
                _context.CommentReplies.Remove(replyComment);
                await _context.SaveChangesAsync();
            }
            else
            {
                
                return null; 
            }

            return replyComment;
        }

        public async Task<GetReplyCommentDTO> EditReplyComment(PostReplyCommentDTO commentReplies, int replyId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var reply = await _context.CommentReplies.FirstOrDefaultAsync(c => c.Id == replyId && c.UserId == userId);

            if (reply == null)
            {
                return null;
            }

            reply.ReplyComment = commentReplies.ReplyComment;
            reply.File = commentReplies.imgUrl;
            
            var commentreplay = new GetReplyCommentDTO
            {
                ReplyComment = reply.ReplyComment,
                file = reply.File,
                CommentSubjectId = reply.CommentSubjectId,
                Id = reply.Id,

                
            };



            await _context.SaveChangesAsync();

            return commentreplay;
        }

        public async Task<List<GetReplyCommentDTO>> GetCommentReplies(int commentSubjectId)
        {
            var replyComments = await _context.CommentReplies
                .Where(rcw => rcw.CommentSubjectId == commentSubjectId)
                .Select(rc => new GetReplyCommentDTO
                {
                    UserName=rc.User.UserName,
                    Id = rc.Id,
                    CommentSubjectId = rc.CommentSubjectId,
                    ReplyComment = rc.ReplyComment,
                    file = rc.File
                })
                .ToListAsync();

            return replyComments;
        }

        public async Task<GetReplyCommentDTO> GetCommentRepliesById(int replyId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _context.Users.FindAsync(userId);
            var replyComment = await _context.CommentReplies
                .FirstOrDefaultAsync(rc => rc.Id == replyId);

            if (replyComment == null)
            {
                return null;
            }

            var dto = new GetReplyCommentDTO
            {
                UserName=user.UserName,
                CommentSubjectId = replyComment.CommentSubjectId,
                ReplyComment = replyComment.ReplyComment,
                file = replyComment.File
            };

            return dto;
        }

 
    }
}
