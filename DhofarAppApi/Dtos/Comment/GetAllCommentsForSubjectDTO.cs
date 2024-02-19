using DhofarAppApi.Dtos.ReplyComment;

namespace DhofarAppApi.Dtos.Comment
{
    public class GetAllCommentsForSubjectDTO
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public int SubjectId { get; set; }

        public string Comment { get; set; }


        public int UpVoteCounter { get; set; }

        public int DownVoteCounter { get; set; }

        public string File { get; set; }

        public string UserImageUrl { get; set; }

        public string Role { get; set; }

        public int RepliesCounter { get; set; }

        public List<GetAllReplyCommentDTO> ReplyComments { get; set; }

    }
}
