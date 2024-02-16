using DhofarAppApi.Data;
using DhofarAppApi.Dtos.Comment;
using DhofarAppApi.Dtos.ReplyComment;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using SixLabors.ImageSharp.Processing;
using System.Security.Claims;
using Image = SixLabors.ImageSharp.Image;


namespace DhofarAppApi.Services
{
    public class CommentSubjectServices : ICommentSubject
    {
        private readonly AppDbContext _db;
        private readonly INotification _notification;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenService;
        public CommentSubjectServices(AppDbContext db, INotification notification, IHttpContextAccessor httpContextAccessor, JWTTokenServices jWTTokenServices)
        {
            _db = db;
            _notification = notification;
            _httpContextAccessor = httpContextAccessor;
            _jWTTokenService = jWTTokenServices;
        }

        public async Task<GetSubjectCommentDTO> CreateCommentSubject(PostCommentDTO postComment, int subjectId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _db.Users.FindAsync(userId);
            var commentSubject = new CommentSubject
            {
                UserId = userId,
                SubjectId = subjectId,
                Comment = postComment.Comment,
                CommentTime = DateTime.UtcNow
            };

            if (postComment.File != null)
            {
                commentSubject.File = postComment.File;
            }
            else
            {
                // Handle case where no file is provided
                // You can skip inserting the file path into the database or provide a default value
                commentSubject.File = ""; // Provide a default value or leave it empty depending on your requirements
            }

           
                await _db.CommentSubjects.AddAsync(commentSubject);
                await _db.SaveChangesAsync();

            //Send notification to the user whose subject received a new comment
            //var user = await _db.Users.FindAsync(userId);
            //var token = await _db.deviceTokens.FirstOrDefaultAsync(t => t.UserId == commentSubject.Subject.UserId);
            //await _notification.SentToUser("New Comment", $"{user.UserName} add comment in your subject ", token.Token);

            var newComment = new GetSubjectCommentDTO
            {
                Comment = commentSubject.Comment,
                UserName = user.UserName,
                File = commentSubject.File,

            };

                return newComment;
           
        }

        public async Task<List<GetSubjectCommentDTO>> GetAllCommentForSubjectId(int subjectId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _db.Users.FindAsync(userId);

            var commentSubjects = await _db.CommentSubjects
                .Where(cs => cs.SubjectId == subjectId)
                .Select(cs => new GetSubjectCommentDTO
                {
                   UserName=user.UserName,
                    Comment = cs.Comment,
                    CommentTime = cs.CommentTime,
                    UpVoteCounter = cs.UpVoteCounter,
                    DownVoteCounter = cs.DownVoteCounter,
                    File = cs.File,
                    ReplyComments = cs.CommentReplies
                    .Where(cr => cr.CommentSubjectId == cs.Id)
                    .Select(cr => new GetReplyCommentDTO
                    {
                      
                        UserName=user.UserName,
                        Id = cr.Id,
                        CommentSubjectId = cr.CommentSubjectId,
                        ReplyComment = cr.ReplyComment
                    }) // check if the commentSubject is the same of the all replies id 
                    .ToList()
                })
                .OrderByDescending(cuv => cuv.UpVoteCounter)
                .ThenBy(cdv => cdv.DownVoteCounter) // Secondary ordering by DownVoteCounter
                .ToListAsync();
            return commentSubjects;
        }


        public async Task VoteUpComment(int commentId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            if (userId != null)
            {
                var existingVote = await _db.UserCommentVotes.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CommentSubjectId == commentId);
                if (existingVote == null)
                {
                    var commentSubject = await _db.CommentSubjects.FindAsync(commentId);
                    if (commentSubject != null)
                    {
                        commentSubject.UpVoteCounter = commentSubject.UpVoteCounter + 1;


                        await _db.UserCommentVotes.AddAsync(new UserCommentVote
                        {
                            CommentSubjectId = commentId,
                            UserId = userId,
                            VoteUp = true,
                            VoteDown = false
                        });
                        _db.Entry(commentSubject).State = EntityState.Modified;

                        await _db.SaveChangesAsync();

                    }
                }
            }

        }



        public async Task VoteDownComment(int commentId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            if (userId != null)
            {
                var existingVote = await _db.UserCommentVotes.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CommentSubjectId == commentId);
                if (existingVote == null)
                {
                    var commentSubject = await _db.CommentSubjects.FindAsync(commentId);
                    if (commentSubject != null)
                    {
                        commentSubject.DownVoteCounter = commentSubject.DownVoteCounter + 1;

                        await _db.UserCommentVotes.AddAsync(new UserCommentVote
                        {
                            CommentSubjectId = commentId,
                            UserId = userId,
                            VoteUp = false,
                            VoteDown = true
                        });

                        _db.Entry(commentSubject).State = EntityState.Modified;

                        await _db.SaveChangesAsync();

                    }
                }
            }

        }

        public async Task RemoveVoteUpComment(int commentId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            if (userId != null)
            {
                var existingVote = await _db.UserCommentVotes.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CommentSubjectId == commentId);
                if (existingVote != null && existingVote.VoteUp)
                {
                    var commentSubject = await _db.CommentSubjects.FindAsync(commentId);
                    if (commentSubject != null)
                    {
                        commentSubject.UpVoteCounter = Math.Max(commentSubject.UpVoteCounter - 1, 0);

                        _db.Entry(existingVote).State = EntityState.Deleted;
                        await _db.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task RemoveVoteDownComment(int commentId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            if (userId != null)
            {
                var existingVote = await _db.UserCommentVotes.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CommentSubjectId == commentId);
                if (existingVote != null && existingVote.VoteDown)
                {
                    var commentSubject = await _db.CommentSubjects.FindAsync(commentId);
                    if (commentSubject != null)
                    {
                        commentSubject.DownVoteCounter = Math.Max(commentSubject.DownVoteCounter - 1, 0);

                        _db.Entry(existingVote).State = EntityState.Deleted;
                        await _db.SaveChangesAsync();
                    }
                }
            }
        }
        public async Task<GetSubjectCommentDTO> EditComment(int commentId,PostCommentDTO commentDTO)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;


            var comment = await _db.CommentSubjects
                .Where(c => c.Id == commentId && c.UserId == userId)
                .FirstOrDefaultAsync();


                comment.Comment = commentDTO.Comment;
               comment.CommentTime = DateTime.UtcNow;
            

            if (commentDTO.File != null && commentDTO.File.Length > 0)
            {
                
                comment.File = commentDTO.File;
            }



            await _db.CommentSubjects.AddAsync(comment);
            await _db.SaveChangesAsync();

            //Send notification to the user whose subject received a new comment
            //var user = await _db.Users.FindAsync(userId);
            //var token = await _db.deviceTokens.FirstOrDefaultAsync(t => t.UserId == commentSubject.Subject.UserId);
            //await _notification.SentToUser("New Comment", $"{user.UserName} add comment in your subject ", token.Token);
            var newComment = new GetSubjectCommentDTO
            {
                Comment = comment.Comment,
                UserName = comment.User.UserName,
                File = comment.File,
                
            };

            return newComment;

        }

        public async Task DeleteComment(int commentId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var comment = await _db.CommentSubjects
                .Where(c => c.Id == commentId && c.UserId == userId)
                .FirstOrDefaultAsync();

            if (comment != null)
            {
                _db.Entry(comment).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
            }
        }

    }
}
