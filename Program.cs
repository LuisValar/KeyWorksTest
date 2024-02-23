using KeyWorksStreamberry.Context;
using KeyWorksStreamberry.Interfaces;
using KeyWorksStreamberry.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ContextMovies>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IMoviesService, MoviesService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
    app.UseHttpsRedirection();

app.MapControllers();

app.Run();

