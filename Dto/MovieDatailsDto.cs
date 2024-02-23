namespace KeyWorksStreamberry.Dto
{
    public class MovieDatailsDto
    {
        public string NomeDoFilme { get; set; }
        public DateTime AnoDeLancamento { get; set; }
        public string GenreName { get; set; }
        public List<string> PlataformasDeStreaming { get; set; }
        public List<string> Comentarios { get; set; }
        public decimal MediaDasNotas { get; set; }
    }
}