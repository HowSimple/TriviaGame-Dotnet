using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using TriviaApp.Contexts;
using TriviaApp.Models;
namespace TriviaApp.Controllers;

public static class TriviaAnswerEndpoints
{
    public static void MapTriviaAnswerEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/TriviaAnswer").WithTags(nameof(TriviaAnswer));

        group.MapGet("/", async (TriviaContext db) =>
        {
            return await db.triviaAnswers.ToListAsync();
        })
        .WithName("GetAllTriviaAnswers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<TriviaAnswer>, NotFound>> (int id, TriviaContext db) =>
        {
            return await db.triviaAnswers.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is TriviaAnswer model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTriviaAnswerById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, TriviaAnswer triviaAnswer, TriviaContext db) =>
        {
            var affected = await db.triviaAnswers
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, triviaAnswer.Id)
                    .SetProperty(m => m.Answer, triviaAnswer.Answer)
                    .SetProperty(m => m.isCorrect, triviaAnswer.isCorrect)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTriviaAnswer")
        .WithOpenApi();

        group.MapPost("/", async (TriviaAnswer triviaAnswer, TriviaContext db) =>
        {
            db.triviaAnswers.Add(triviaAnswer);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/TriviaAnswer/{triviaAnswer.Id}", triviaAnswer);
        })
        .WithName("CreateTriviaAnswer")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, TriviaContext db) =>
        {
            var affected = await db.triviaAnswers
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTriviaAnswer")
        .WithOpenApi();
    }
}