namespace KeyWorksStreamberry.Dto
{
    public class MovieDto
    {
        public string Title { get; set; }
        public DateTime YearRelase { get; set; }
        public string Genere { get; set; }
        public decimal NotaCreate { get; set; }
        public string Comment { get; set; }
        public List<string> Streamings { get; set; }
    }
}