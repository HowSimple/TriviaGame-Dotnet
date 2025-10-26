using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Options;
using TriviaApp;
using TriviaApp.Contexts;
using TriviaApp.Controllers;
using TriviaApp.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.Services.AddOpenApi();

        // Add services to the container.


        builder.Services.AddDbContext<TriviaContext>(options =>
        
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))


            //opt.UseInMemoryDatabase("Trivia"));
            //options.UseSqlServer(connectionString))
        );
        builder.Services.AddScoped<TriviaFetchService>();


        builder.Services.AddHttpClient();
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

                    app.MapTriviaAnswerEndpoints();
                    app.MapTriviaQuestionEndpoints();


        app.Run();
    }
}