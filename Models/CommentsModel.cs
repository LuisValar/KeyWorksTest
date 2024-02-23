
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyWorksStreamberry.Models
{
    public class CommentsModel
    {
        [Key]
        public int id { get; set; }
        public int MovieId { get; set; }
        public decimal Nota { get; set; }
        public string Comment { get; set; }
    }
}