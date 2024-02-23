using System.ComponentModel.DataAnnotations;


namespace KeyWorksStreamberry.Models
{
    public class StreamingsMovie
    {
        [Key]
        public int Id { get; set;}
        public int MovieId { get; set; }
        public string StreamingName { get; set; }
    }
}