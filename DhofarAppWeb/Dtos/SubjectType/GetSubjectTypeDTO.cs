﻿using DhofarAppWeb.Dtos.Subject;

namespace DhofarAppWeb.Dtos.SubjectType
{
    public class GetSubjectTypeDTO
    {
        public string Name { get; set; }

        public string TitleValue { get; set; }

        public List<GetSubjectDTO> Subjects { get; set; }
    }
}
