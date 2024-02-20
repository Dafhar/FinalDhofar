using DhofarAppApi.Data;
using DhofarAppApi.Dtos.Comment;
using DhofarAppApi.Dtos.Complaint;
using DhofarAppApi.Dtos.ComplaintFiles;
using DhofarAppApi.Dtos.GeneralSubject;
using DhofarAppApi.Dtos.RatingSubjectDTO;
using DhofarAppApi.Dtos.ReplyComment;
using DhofarAppApi.Dtos.Subject;
using DhofarAppApi.Dtos.SubjectFiles;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using DhofarAppApi.Services;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace DhofarAppWeb.Services
{
    public class SubjectServices : ISubject
    {
        private readonly AppDbContext _db;
        private readonly INotification _notification;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenService;
        public SubjectServices(AppDbContext db, INotification notification, IHttpContextAccessor httpContextAccessor, JWTTokenServices jWTTokenService)
        {
            _db = db;
            _notification = notification;
            _httpContextAccessor = httpContextAccessor;
            _jWTTokenService = jWTTokenService;
        }

        public async Task<GetSubjectDTO> CreateSubject(PostSubjectDTO postSubjectDto, int generalSubjectTypeId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            if (userId == null)
                return null;

            var subject = new Subject
            {
                UserId = userId,
                PrimarySubject = postSubjectDto.PrimarySubject,
                GeneralSubjectsTypesId = generalSubjectTypeId,
                Title = postSubjectDto.Title,
                Description = postSubjectDto.Description,
                CreatedTime = DateTime.Now,
                Files = new List<SubjectFiles>()
            };
            if (!string.IsNullOrEmpty(postSubjectDto.PollQuestion) && postSubjectDto.PollOptions != null)
            {
                // Create a poll for the subject
                var poll = new Poll
                {
                    Question = postSubjectDto.PollQuestion,
                    Options = postSubjectDto.PollOptions.Select(optionText => new PollOption { OptionText = optionText }).ToList()
                };

                subject.Poll = poll; // Associate the poll with the subject
            }

            await _db.Subjects.AddAsync(subject);
            await _db.SaveChangesAsync();

            foreach (var item in postSubjectDto.SubjectTypeId)
            {
                SubjectTypeSubject subjectTypeSubject = new SubjectTypeSubject()
                {
                    SubjectId = subject.Id,
                    SubjectTypeId = item,
                };
                await _db.SubjectTypeSubjects.AddAsync(subjectTypeSubject);
            }

            await _db.SaveChangesAsync();



            if (postSubjectDto.ImageUrl != null)
            {
                foreach (var file in postSubjectDto.ImageUrl)
                {
                    if (file != null && file.Length > 0)
                    {

                        var newSubjectFile = new SubjectFiles
                        {
                            FilePaths = file,
                            SubjectId = subject.Id
                        };

                        await _db.SubjectFiles.AddAsync(newSubjectFile);
                        await _db.SaveChangesAsync();
                    }
                }
            }

            var getSubjectDto = new GetSubjectDTO
            {
                Id = subject.Id,
                UserName = decodedJwt.Claims.FirstOrDefault(c => c.Type == "unique_name").Value, // Add null check here
                PrimarySubject = subject.PrimarySubject,
                CreatedTime = subject.CreatedTime,
                Title = subject.Title,
                Description = subject.Description,
                VisitorCounter = subject.VisitorCounter,
                Poll = subject.Poll != null ? new PollDTO
                {
                    Id = subject.Poll.Id,
                    SubjectId = subject.Poll.SubjectId,
                    Question = subject.Poll.Question,
                    Options = subject.Poll.Options.Select(po => new PollOptionDTO
                    {
                        Id = po.Id,
                        PollId = po.PollId,
                        OptionText = po.OptionText,
                        VoteCount = po.VoteCount
                    }).ToList()
                } : null, // Add null check here
                Files = subject.Files.Select(fi => new GetSubjectFilesDTO
                {
                    Id = fi.Id,
                    SubjectId = fi.SubjectId,
                    FilePaths = fi.FilePaths
                }).ToList(),
            };
            return getSubjectDto;
        }

        public async Task<List<GetSubjectDTO>> GetAllSubjects()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _db.Users.FindAsync(userId);



            var subjects = await _db.Subjects

                .Select(s => new GetSubjectDTO
                {
                    Id = s.Id,
                    UserName = s.User.UserName,
                    PrimarySubject = s.PrimarySubject,
                    SubjectType_Name = s.SubjectTypeSubjects.Where(sts => sts.SubjectId == s.Id)
                    .Select(sts => user.SelectedLanguage == "en" ? sts.SubjectType.Name_En : sts.SubjectType.Name_Ar).ToList(),
                    Title = s.Title,
                    Description = s.Description,
                    LikeCounter = s.LikeCounter,
                    DisLikeCounter = s.DisLikeCounter,
                    VisitorCounter = s.VisitorCounter,
                    CreatedTime = s.CreatedTime,
                    CommentsCount = s.CommentSubjects.Count(),
                    Files = s.Files.Select(fi => new GetSubjectFilesDTO
                    {
                        FilePaths = fi.FilePaths,
                    }).ToList(),
                    CommentsSubjects = s.CommentSubjects
                    .Select(cs => new GetSubjectCommentDTO
                    {
                        Id = cs.Id,
                        SubjectId = cs.SubjectId,
                        UserName = cs.User.UserName,
                        UserImageUrl = cs.User.ImageURL,
                        Comment = cs.Comment,
                        File = cs.File,
                        ReplyComments = cs.CommentReplies.Where(crr => crr.CommentSubjectId == cs.Id).Select(cr => new GetReplyCommentDTO
                        {
                            Id = cr.Id,
                            UserName = cr.User.UserName,
                            UserImageUrl = cr.User.ImageURL,
                            CommentSubjectId = cr.CommentSubjectId,
                            ReplyComment = cr.ReplyComment,
                            
                            file = cr.File
                        })
                        .ToList(),
                    }).ToList(),
                    Poll = s.Poll != null ? new PollDTO
                    {
                        Id = s.Poll.Id,
                        SubjectId = s.Poll.SubjectId,
                        Question = s.Poll.Question,
                        Options = s.Poll.Options.Select(po => new PollOptionDTO
                        {
                            Id = po.Id,
                            PollId = po.PollId,
                            OptionText = po.OptionText,
                            VoteCount = po.VoteCount
                        }).ToList()
                    } : null
                })
                .ToListAsync();
            return subjects;
        }

        public async Task<GetSubjectDTO> GetSubjectById(int Id)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _db.Users.FindAsync(userId);

            var subject = await _db.Subjects
                .Where(s => s.Id == Id)
                .Select(s => new GetSubjectDTO
                {
                    Id = s.Id,
                    UserName = s.User.UserName,
                    PrimarySubject = s.PrimarySubject,
                    SubjectType_Name = s.SubjectTypeSubjects.Select(sts => user.SelectedLanguage == "en" ? sts.SubjectType.Name_En : sts.SubjectType.Name_Ar).ToList(),
                    Title = s.Title,
                    Description = s.Description,
                    LikeCounter = s.LikeCounter,
                    DisLikeCounter = s.DisLikeCounter,
                    VisitorCounter = s.VisitorCounter,
                    CreatedTime = s.CreatedTime,
                    
                    Files = s.Files.Select(fi => new GetSubjectFilesDTO
                    {
                        FilePaths = fi.FilePaths,
                    }).ToList(),
                    CommentsSubjects = s.CommentSubjects
                    .Select(cs => new GetSubjectCommentDTO
                    {
                        Id = cs.Id,
                        SubjectId = cs.SubjectId,
                        UserName = cs.User.UserName,
                        UserImageUrl = cs.User.ImageURL,
                        Comment = cs.Comment,
                        File = cs.File,
                        ReplyComments = cs.CommentReplies.Where(crr => crr.CommentSubjectId == cs.Id).Select(cr => new GetReplyCommentDTO
                        {
                            Id = cr.Id,
                            UserName = cr.User.UserName,
                            UserImageUrl = cr.User.ImageURL,
                            CommentSubjectId = cr.CommentSubjectId,
                            ReplyComment = cr.ReplyComment,
                            file = cr.File
                        })
                        .ToList(),
                    }).ToList(),
                    Poll = s.Poll != null ? new PollDTO
                    {
                        Id = s.Poll.Id,
                        SubjectId = s.Poll.SubjectId,
                        Question = s.Poll.Question,
                        Options = s.Poll.Options.Select(po => new PollOptionDTO
                        {
                            Id = po.Id,
                            PollId = po.PollId,
                            OptionText = po.OptionText,
                            VoteCount = po.VoteCount
                        }).ToList()
                    } : null
                })
                .FirstOrDefaultAsync();

            return subject;
        }

        public void IncrementVisitorCounter(int subjectId)
        {
            var subject = _db.Subjects.Find(subjectId);
            if (subject != null)
            {
                subject.VisitorCounter++;
                _db.SaveChanges();
            }
        }

        public int getVisitorCounter(int subjectId)
        {
            var subject = _db.Subjects.Find(subjectId);
            return subject?.VisitorCounter ?? 0;
        }

        public async Task<Subject> DeleteSubject(int Id)
        {
            var subject = await _db.Subjects.FindAsync(Id);
            if (subject != null)
            {
                _db.Entry(subject).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
            }
            return subject;
        }

        public async Task<List<GetSubjectDTO>> GetSubjectByUserId()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _db.Users.FindAsync(userId);


            if (userId == null)
                return null;

            var mySubjects = await _db.Subjects.Where(su => su.UserId == userId)

                .Select(s => new GetSubjectDTO
                {
                    Id = s.Id,
                    PrimarySubject = s.PrimarySubject,
                    SubjectType_Name = s.SubjectTypeSubjects.Select(sts => user.SelectedLanguage == "en" ? sts.SubjectType.Name_En : sts.SubjectType.Name_Ar).ToList(),
                    Title = s.Title,
                    Description = s.Description,
                    VisitorCounter = s.VisitorCounter,
                    CreatedTime = s.CreatedTime,
                    CommentsSubjects = s.CommentSubjects.Select(cs => new GetSubjectCommentDTO
                    {
                        Id = cs.Id,
                        SubjectId = cs.SubjectId,
                        UserName = cs.User.UserName,
                        Comment = cs.Comment,
                        UserImageUrl = cs.User.ImageURL

                    }).ToList()
                })
                .ToListAsync();

            return mySubjects;
        }

        public async Task<GetSubjectDTO> EditSubject(int subjectId, EditSubjectDTO postSubjectDto)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _db.Users.FindAsync(userId);

            var subjectToEdit = await _db.Subjects
                .Where(c => c.Id == subjectId && c.UserId == userId)
                .Include(sf => sf.Files)
                .FirstOrDefaultAsync();
            if (subjectToEdit == null)
            {
                return null;
            }

            // Update subject properties
            subjectToEdit.PrimarySubject = postSubjectDto.PrimarySubject;
            subjectToEdit.Title = postSubjectDto.Title;
            subjectToEdit.Description = postSubjectDto.Description;

            // Handle poll updates or creation if applicable
            if (!string.IsNullOrEmpty(postSubjectDto.PollQuestion) && postSubjectDto.PollOptions != null)
            {
                // Create a poll for the subject
                var poll = new Poll
                {
                    Question = postSubjectDto.PollQuestion,
                    Options = postSubjectDto.PollOptions.Select(optionText => new PollOption { OptionText = optionText }).ToList()
                };

                subjectToEdit.Poll = poll; // Associate the poll with the subject
            }
            if (subjectToEdit.Files == null)
            {
                subjectToEdit.Files = new List<SubjectFiles>();
            }
            // Handle file updates or creation if applicable
            if (postSubjectDto.ImagUrl != null)
            {
                foreach (var file in postSubjectDto.ImagUrl)
                {
                    if (file != null && file.Length > 0)
                    {

                        var newSubjectFile = new SubjectFiles
                        {
                            FilePaths = file,
                            SubjectId = subjectToEdit.Id
                        };
                        subjectToEdit.Files.Add(newSubjectFile);

                    }
                }
            }

            await _db.SaveChangesAsync();

            // Construct and return response DTO
            var getSubjectDto = new GetSubjectDTO
            {
                Id = subjectToEdit.Id,
                PrimarySubject = subjectToEdit.PrimarySubject,
                SubjectType_Name = subjectToEdit.SubjectTypeSubjects.Select(sts => user.SelectedLanguage == "en" ? sts.SubjectType.Name_En : sts.SubjectType.Name_Ar).ToList(),
                Title = subjectToEdit.Title,
                Description = subjectToEdit.Description,
                VisitorCounter = subjectToEdit.VisitorCounter,
                Files = subjectToEdit.Files.Select(fi => new GetSubjectFilesDTO
                {
                    FilePaths = fi.FilePaths,
                    SubjectId = fi.SubjectId,
                    Id = fi.Id
                }).ToList()
            };

            return getSubjectDto;
        }

        public async Task<string> Like(int subjectId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var selectedSub = await _db.Subjects.FindAsync(subjectId);
            var user = await _db.Users.FindAsync(userId);
            var token = await _db.deviceTokens.FirstOrDefaultAsync(t => t.UserId == selectedSub.UserId);

            if (selectedSub == null)
            {
                return "Subject not found";
            }

            var existingRating = await _db.RatingSubjects.FirstOrDefaultAsync(r => r.SubjectId == subjectId && r.UserId == userId);
            if (existingRating != null && existingRating.IsLike)
            {
                selectedSub.LikeCounter--;
                _db.Entry(selectedSub).State = EntityState.Modified;
                _db.RatingSubjects.Remove(existingRating);
                await _db.SaveChangesAsync();
                return $"You unliked the subject: {selectedSub.Title}";
            }

            if (existingRating != null && existingRating.IsDisLike)
            {
                existingRating.IsLike = true;
                existingRating.IsDisLike = false;
                selectedSub.DisLikeCounter--;
                selectedSub.LikeCounter++;

                _db.Entry(selectedSub).State = EntityState.Modified;
                _db.Entry(existingRating).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                // await _notification.SentToUser("Like", $"{user.UserName} like your subject ", token.Token);


                return $"You liked the subject: {selectedSub.Title}";
            }

            selectedSub.LikeCounter++;
            _db.Entry(selectedSub).State = EntityState.Modified;

            var newRating = new RatingSubject()
            {
                SubjectId = subjectId,
                IsLike = true,
                UserId = userId
            };
            await _db.RatingSubjects.AddAsync(newRating);

            await _db.SaveChangesAsync();
            //   await _notification.SentToUser("Like", $"{user.UserName} like your subject ", token.Token);

            return $"You liked the subject: {selectedSub.Title}";
        }
        public async Task<string> Dislike(int subjectId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var selectedSub = await _db.Subjects.FindAsync(subjectId);
            var user = await _db.Users.FindAsync(userId);
            var token = await _db.deviceTokens.FirstOrDefaultAsync(t => t.UserId == selectedSub.UserId);

            if (selectedSub == null)
            {
                return "Subject not found";
            }

            var existingRating = await _db.RatingSubjects.FirstOrDefaultAsync(r => r.SubjectId == subjectId && r.UserId == userId);
            if (existingRating != null && existingRating.IsDisLike)
            {
                // User has already disliked the subject, so remove the dislike
                selectedSub.DisLikeCounter--;
                _db.Entry(selectedSub).State = EntityState.Modified;
                _db.RatingSubjects.Remove(existingRating); // Remove the existing dislike rating
                await _db.SaveChangesAsync();
                return $"You removed your dislike for the subject: {selectedSub.Title}";
            }

            if (existingRating != null && existingRating.IsLike)
            {
                // User has previously liked the subject, so update the rating to reflect dislike
                existingRating.IsLike = false;
                existingRating.IsDisLike = true;
                selectedSub.DisLikeCounter++;
                selectedSub.LikeCounter--;
                _db.Entry(selectedSub).State = EntityState.Modified;
                _db.Entry(existingRating).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                //  await _notification.SentToUser("React", $"{user.UserName} react at your subject ", token.Token);


                return $"You Disliked the subject: {selectedSub.Title}";
            }

            selectedSub.DisLikeCounter++;
            _db.Entry(selectedSub).State = EntityState.Modified;

            var newRating = new RatingSubject()
            {
                SubjectId = subjectId,
                IsDisLike = true,
                UserId = userId
            };
            await _db.RatingSubjects.AddAsync(newRating);

            await _db.SaveChangesAsync();
            // await _notification.SentToUser("React", $"{user.UserName} react at your subject ", token.Token);

            return $"You disliked the subject: {selectedSub.Title}";
        }


        public async Task AddSubjectToFavorite(int subjectId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            // Check if the subject is not already in favorites
            if (!_db.FavoriteSubjects.Any(f => f.SubjectId == subjectId && f.UserId == userId))
            {
                var favoriteSubject = new FavoriteSubject
                {
                    SubjectId = subjectId,
                    UserId = userId
                };

                _db.FavoriteSubjects.Add(favoriteSubject);
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveSubjectFromFavorite(int subjectId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var favoriteSubject = await _db.FavoriteSubjects
                .FirstOrDefaultAsync(f => f.SubjectId == subjectId && f.UserId == userId);

            if (favoriteSubject != null)
            {
                _db.FavoriteSubjects.Remove(favoriteSubject);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> VoteForPollOption(int subjectId, int PollId, int pollOptionId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            if (userId == null)
            {
                return false; // Voting requires a valid user
            }

            var subject = await _db.Subjects
                .Include(s => s.Poll)
                .ThenInclude(p => p.Options)
                .FirstOrDefaultAsync(s => s.Id == subjectId);

            if (subject == null || subject.Poll == null)
            {
                return false; // Subject or poll not found
            }

            var pollOption = subject.Poll.Options.FirstOrDefault(po => po.Id == pollOptionId);

            if (pollOption == null)
            {
                return false; // Poll option not found
            }

            // Check if the user has already voted for this poll option
            bool userHasVoted = await _db.UserVotes
                .AnyAsync(uv => uv.UserId == userId && uv.PollId == PollId);

            if (userHasVoted)
            {
                return false; // User has already voted for this poll option
            }

            // Update the vote count for the poll option
            pollOption.VoteCount++;

            // Record the user's vote
            var userVote = new UserVote
            {
                UserId = userId,
                PollId = PollId,
                PollOptionId = pollOptionId
            };

            await _db.UserVotes.AddAsync(userVote);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<ListOfAllSubjectByGeneralSubjectTypeDTO>> GetFavoriteSubjects()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _db.Users.FindAsync(userId);

            var favoriteSubjects = await _db.FavoriteSubjects
                .Where(f => f.UserId == userId)
                .Select(f => new ListOfAllSubjectByGeneralSubjectTypeDTO
                {
                    Id = f.Subject.Id,
                    SubjectTypeName = f.Subject.SubjectTypeSubjects.Select(sts => user.SelectedLanguage == "en" ? sts.SubjectType.Name_En : sts.SubjectType.Name_Ar).ToList(),
                    PrimarySubject = f.Subject.PrimarySubject,
                    File = f.Subject.Files.Select(fs => new GetSubjectFilesDTO
                    {
                        FilePaths = fs.FilePaths
                    }).FirstOrDefault(),
                    UsersImagesUrl = f.Subject.CommentSubjects.Select(cs => cs.User.ImageURL).Distinct().ToList(),
                    CreatedTime = f.Subject.CreatedTime,
                    CommentsCount = f.Subject.CommentSubjects.Count
                })
                .ToListAsync();
            return favoriteSubjects;
        }


        public async Task<List<PollDTO>> GetPollBySubject(int subjectId)
        {
            var polls = await _db.Polls
                .Where(p => p.SubjectId == subjectId)
                .Select(p => new PollDTO
                {
                    Id = p.Id,
                    SubjectId = p.SubjectId,
                    Question = p.Question,
                    Options = p.Options.Where(po => po.PollId == p.Id).Select(po => new PollOptionDTO
                    {
                        Id = po.Id,
                        PollId = po.PollId,
                        OptionText = po.OptionText,
                        VoteCount = po.VoteCount
                    }).ToList()

                }).ToListAsync();
            if (polls != null)
            {
                return polls;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteVote(int subjectId, int pollId, int pollOptionId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;


            if (userId == null)
            {
                return false; // Deleting a vote requires a valid user
            }

            var userVote = await _db.UserVotes
                .FirstOrDefaultAsync(uv => uv.UserId == userId && uv.PollId == pollId && uv.PollOptionId == pollOptionId);

            if (userVote == null)
            {
                return false; // User vote not found
            }

            // Update the vote count for the poll option
            var pollOption = await _db.PollOptions
                .FirstOrDefaultAsync(po => po.Id == pollOptionId);

            if (pollOption != null)
            {
                if (pollOption.VoteCount > 0)
                {
                    pollOption.VoteCount--;
                }
            }

            // Remove the user's vote
            _db.UserVotes.Remove(userVote);
            await _db.SaveChangesAsync();

            return true;
        }


        public async Task<List<TopRatedSubjectsDTO>> GetTheMostSubjectInteract()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenService.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;

            var user = await _db.Users.FindAsync(userId);

            var subject = await _db.Subjects
                .OrderByDescending(s => s.LikeCounter + s.CommentSubjects.Count)
                .ThenBy(s => s.VisitorCounter)
                .Select(s => new TopRatedSubjectsDTO
                {
                    Id = s.Id,
                    FullName = s.User.UserName,
                    UserImageUrl = s.User.ImageURL,
                    CreatedTime = s.CreatedTime,
                    PrimarySubject = s.PrimarySubject,
                    Description = s.Description,
                    File = s.Files.Select(fi => new GetSubjectFilesDTO
                    {
                        FilePaths = fi.FilePaths,
                    }).FirstOrDefault(),
                })
                .Take(2)
                .ToListAsync();
            if (subject != null)
            {
                return subject;
            }
            else
            {
                return null;
            }

        }



        public async Task<int> GetCountOfSubjects()
        {
            var subjectCount = await _db.Subjects.CountAsync();

            return subjectCount;
        }

    }
}
