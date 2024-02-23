using System.ComponentModel.DataAnnotations;

namespace KeyWorksStreamberry.Models
{
    public class MovieModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime YearRelase { get; set; } 
        public string Genere { get; set; }
        public ICollection<StreamingsMovie> Streaming { get; set; }
        public ICollection<CommentsModel> Comments { get; set; }
    }
}