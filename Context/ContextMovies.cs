
using KeyWorksStreamberry.Models;
using Microsoft.EntityFrameworkCore;

namespace KeyWorksStreamberry.Context
{
    public class ContextMovies : DbContext
    {
        public ContextMovies(DbContextOptions<ContextMovies> options) : base(options){}
        public DbSet<MovieModel> Movies { get; set; }
        public DbSet<CommentsModel> Comments { get; set; }
        public DbSet<StreamingsMovie> Streamings { get; set; }
    }
}