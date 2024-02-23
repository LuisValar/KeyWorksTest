
using KeyWorksStreamberry.Dto;

namespace KeyWorksStreamberry.Interfaces
{
    public interface IMoviesService
    {
        void AddCommentAndRating(CommentAndRatingDto commentAndRating);
        void CreateMovie(MovieDto movie);
        void DeleteMovie(int movieId);
        List<MovieDatailsDto> GetMoviesWithDetails(int pageIndex, int pageSize);
        List<GenreRatingDto> GetAverageRatingsByGenreAndReleaseYear();
        bool MovieExists(int Id);
        List<MovieDatailsDto> SearchMovies(string movieName, DateTime? releaseYear, string? genre, int pageIndex, int pageSize);
        List<MovieDatailsDto> SearchMoviesGerene(string genre, DateTime releaseYear, int pageIndex, int pageSize);
        void UpdateMovie(int movieId, UpdateMovieDto updateMovieDto);
    }
}