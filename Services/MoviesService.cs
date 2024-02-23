using KeyWorksStreamberry.Context;
using KeyWorksStreamberry.Dto;
using KeyWorksStreamberry.Interfaces;
using KeyWorksStreamberry.Models;
using Microsoft.EntityFrameworkCore;

namespace KeyWorksStreamberry.Services
{
    public class MoviesService : IMoviesService
    {
         private readonly ContextMovies _dbContext;

        public MoviesService (ContextMovies contextMovies)
        {
            _dbContext = contextMovies;
        }

        public void AddCommentAndRating(CommentAndRatingDto commentAndRating)
        {
            try
            {
                var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == commentAndRating.MovieId);
                if (movie == null)
                {
                    throw new InvalidOperationException("MovieId não encontrado na tabela Movies.");
                }

                var newComment = new CommentsModel
                {
                    MovieId = commentAndRating.MovieId,
                    Comment = commentAndRating.Comentario,
                    Nota = commentAndRating.Nota
                };

                _dbContext.Comments.Add(newComment);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao adicionar comentário e classificação: {ex.Message}", ex);
            }
        }
        public void CreateMovie(MovieDto movie)
        {
            try
            {
                if (movie.NotaCreate < 0 || movie.NotaCreate > 5)
                throw new ArgumentException("A nota do filme deve estar entre 0 e 5.");

                var newMovie = new MovieModel
                {
                    Title = movie.Title,
                    YearRelase = movie.YearRelase, 
                    Genere = movie.Genere
                };

                _dbContext.Movies.Add(newMovie);
                _dbContext.SaveChanges();

                var comment = new CommentsModel
                {
                    MovieId = newMovie.Id,
                    Nota = movie.NotaCreate, 
                    Comment = movie.Comment
                };

                _dbContext.Comments.Add(comment);
                _dbContext.SaveChanges();

                foreach (var plataforma in movie.Streamings)
                {
                    var streamingsMovie = new StreamingsMovie
                    {
                        MovieId = newMovie.Id,
                        StreamingName = plataforma
                    };

                    _dbContext.Streamings.Add(streamingsMovie);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao criar um filme: {ex.Message}", ex);
            }
        }

        public List<MovieDatailsDto> GetMoviesWithDetails(int pageIndex, int pageSize)
        {
            try
            {
                var movies = _dbContext.Movies
                .Include(m => m.Streaming)
                .Include(m => m.Comments)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                var moviesDto = movies.Select(m => new MovieDatailsDto
                {
                    NomeDoFilme = m.Title,
                    AnoDeLancamento = m.YearRelase,
                    GenreName = m.Genere,
                    Comentarios = GetCommentsByMovieId(m.Id),
                    PlataformasDeStreaming = GetStreamingsByMovieId(m.Id),
                    MediaDasNotas = GetAverageRatingByMovieId(m.Id)
                }).ToList();

                return moviesDto;
            }
            
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao procurar os filmes: {ex.Message}", ex);
            }
        }

        public List<GenreRatingDto> GetAverageRatingsByGenreAndReleaseYear()
        {
            try
            {
                var result = _dbContext.Movies
                .GroupJoin(
                    _dbContext.Comments,
                    movie => movie.Id,
                    comment => comment.MovieId,
                    (movie, comments) => new { Movie = movie, Comments = comments }
                )
                .SelectMany(
                    x => x.Comments.DefaultIfEmpty(),
                    (movie, comment) => new { Movie = movie.Movie, Comment = comment }
                )
                .GroupBy(
                    x => new { Genre = x.Movie.Genere, ReleaseYear = x.Movie.YearRelase },
                    (key, group) => new GenreRatingDto
                    {
                        Genre = key.Genre,
                        ReleaseYear = key.ReleaseYear,
                        AverageRating = group.Any() ? group.Average(x => x.Comment != null ? x.Comment.Nota : 0) : 0
                    }
                )
                .ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao procurar os filmes com melhores avaliações: {ex.Message}", ex);
            }            
        }

        public List<MovieDatailsDto> SearchMovies(string movieName, DateTime? releaseYear, string? genre, int pageIndex, int pageSize)
        {
            try
            {
                var query = _dbContext.Movies
                .Include(m => m.Streaming)
                .Include(m => m.Comments)
                .AsQueryable();

                if (!string.IsNullOrEmpty(movieName))
                    query = query.Where(m => m.Title.Contains(movieName));
                

                if (releaseYear.HasValue)
                    query = query.Where(m => m.YearRelase == releaseYear.Value);
                

                if (!string.IsNullOrEmpty(genre))
                    query = query.Where(m => m.Genere == genre);

                var moviesDto = query
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(m => new MovieDatailsDto
                    {
                        NomeDoFilme = m.Title,
                        AnoDeLancamento = m.YearRelase,
                        GenreName = m.Genere,
                        Comentarios = m.Comments.Select(c => c.Comment).ToList(),
                        MediaDasNotas = m.Comments.Any() ? m.Comments.Average(c => c.Nota) : 0,
                        PlataformasDeStreaming = m.Streaming.Select(s => s.StreamingName).ToList()
                    })
                    .ToList();

                return moviesDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao procurar o filme: {ex.Message}", ex);
            } 
        }

        public List<MovieDatailsDto> SearchMoviesGerene(string genre, DateTime yearRealase, int pageIndex, int pageSize)
        {
            try
            {
                var movies = _dbContext.Movies
                .Include(m => m.Streaming)
                .Include(m => m.Comments)
                .Where(m => m.Genere == genre)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

                var moviesDto = movies.Select(m => new MovieDatailsDto
                {
                    NomeDoFilme = m.Title,
                    AnoDeLancamento = m.YearRelase,
                    GenreName = m.Genere,
                    Comentarios = GetCommentsByMovieId(m.Id),
                    PlataformasDeStreaming = GetStreamingsByMovieId(m.Id),
                    MediaDasNotas = GetAverageRatingByMovieId(m.Id)
                }).ToList();

                return moviesDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao buscar o filme pelo genero/ano de lançamento: {ex.Message}", ex);
            } 
        }

         public void UpdateMovie(int movieId, UpdateMovieDto updateMovieDto)
         {
            try
            {
                var existingMovie = _dbContext.Movies.Find(movieId);
                if (existingMovie == null)
                    throw new Exception($"Filme com ID {movieId} não encontrado.");
        
                existingMovie.Title = updateMovieDto.Title;
                existingMovie.YearRelase = updateMovieDto.YearRelase;
                existingMovie.Genere = updateMovieDto.Genre;

                var existingMovieStreamings = _dbContext.Streamings.Where(ms => ms.MovieId == movieId).ToList();
                _dbContext.Streamings.RemoveRange(existingMovieStreamings);

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao atualizar o Filme: {ex.Message}", ex);
            }
         }


         public void DeleteMovie(int movieId)
        {
            try
            {
                var movie = _dbContext.Movies.Find(movieId);

                if (movie != null)
                {
                    _dbContext.Movies.Remove(movie);
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception($"Filme com ID {movieId} não encontrado.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao Deletar o Filme: {ex.Message}", ex);
            }
            
        }

        public List<string> GetCommentsByMovieId(int movieId)
        {
            var comments = _dbContext.Comments
                .Where(c => c.MovieId == movieId)
                .Select(c => c.Comment)
                .ToList();

            return comments;
        }

        public List<string> GetStreamingsByMovieId(int movieId)
        {
            var streamings = _dbContext.Streamings
                .Where(s => s.MovieId == movieId)
                .Select(s => s.StreamingName)
                .ToList();

            return streamings;
        }
        public decimal GetAverageRatingByMovieId(int movieId)
        {
            var averageRating = _dbContext.Comments
                .Where(c => c.MovieId == movieId)
                .Any() ? _dbContext.Comments
                    .Where(c => c.MovieId == movieId)
                    .Average(c => c.Nota) : 0;

            return averageRating;
        }

        public bool MovieExists (int Id)
        {
            return _dbContext.Movies.Any(m => m.Id == Id);
        }
        
    }
}