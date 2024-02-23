namespace KeyWorksStreamberry.Dto
{
    public class UpdateMovieDto
    {
        public string Title { get; set; }
        public DateTime YearRelase { get; set; }
        public string Genre { get; set; } 
        public List<string> PlataformaStreaming { get; set; }
    }
}