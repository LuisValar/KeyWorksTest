using KeyWorksStreamberry.Dto;
using KeyWorksStreamberry.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KeyWorksStreamberry.Controllers
{
    [ApiController]
    [Route("Movie")]  
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;

        public MoviesController (IMoviesService movieService)
        {
            _moviesService = movieService;
        }

        [HttpPost("PostCommentAndRating")]
        public IActionResult PostCommentAndRating([FromBody] CommentAndRatingDto commentAndRating)
        {
            try
            {
                _moviesService.AddCommentAndRating(commentAndRating);
                return Ok("Comentário e nota cadastrados com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar o comentário e a nota: {ex.Message}");
            }
        }
        
        [HttpPost("CreateMovie")]
        public IActionResult CreateMovie([FromBody]MovieDto movie)
        {
             try
            {
                _moviesService.CreateMovie(movie);
                return Ok("Filme cadastrado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao cadastrar o filme: {ex.Message}");
            }
        }

        [HttpGet("GetAllMovies")]
        public IActionResult GetMovies(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var moviesDto = _moviesService.GetMoviesWithDetails(pageIndex, pageSize);
                return Ok(moviesDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter a lista de filmes: {ex.Message}");
            }
        }

        [HttpGet("Search/Genre")]
        public IActionResult SearchMovies(string genre, DateTime releaseYear, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var moviesDto = _moviesService.SearchMoviesGerene(genre, releaseYear, pageIndex, pageSize);
                return Ok(moviesDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao pesquisar filmes: {ex.Message}");
            }
        }

        [HttpGet("Search")]
        public IActionResult SearchMovies(string searchTerm, DateTime? releaseYear, string? genre, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var moviesDto = _moviesService.SearchMovies(searchTerm, releaseYear, genre, pageIndex, pageSize);
                return Ok(moviesDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao pesquisar filmes: {ex.Message}");
            }
        }

        [HttpGet("AverageRatingsByGenreAndReleaseYear")]
        public IActionResult GetAverageRatingsByGenreAndReleaseYear()
        {
            try
            {
                var result = _moviesService.GetAverageRatingsByGenreAndReleaseYear();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter avaliações médias: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, [FromBody] UpdateMovieDto updateMovieDto)
        {
            try
            {
                var existingMovie = _moviesService.MovieExists(id);
                if (!existingMovie)
                    return NotFound($"Filme com ID {id} não encontrado.");

                _moviesService.UpdateMovie(id, updateMovieDto);

                return Ok("Filme atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar o filme: {ex.Message}");
            }
        }

         [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            try
            {
                _moviesService.DeleteMovie(id);
                return Ok("Filme excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir o filme: {ex.Message}");
            }
        }
        
    }
}