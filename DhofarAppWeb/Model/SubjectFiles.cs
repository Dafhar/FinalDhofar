using System.ComponentModel.DataAnnotations.Schema;

namespace DhofarAppWeb.Model
{
    public class SubjectFiles
    {
        public int Id { get; set; }

        public string? FilePaths { get; set; }


        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        public Subject? Subject { get; set; }
    }
}
