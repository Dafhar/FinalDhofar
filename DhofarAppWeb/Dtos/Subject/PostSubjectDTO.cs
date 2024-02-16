﻿using DhofarAppWeb.Dtos.ComplaintFiles;
using DhofarAppWeb.Dtos.SubjectFiles;

namespace DhofarAppWeb.Dtos.Subject
{
    public class PostSubjectDTO
    {
        public required string PrimarySubject { get; set; }

        public int SubjectTypeId { get; set; }

        public required string Title { get; set; }


        public required string Description { get; set; }
        public string? PollQuestion { get; set; }

        public List<string>? PollOptions { get; set; }
        public List<string>? ImageUrl { get; set; }


    }
}
