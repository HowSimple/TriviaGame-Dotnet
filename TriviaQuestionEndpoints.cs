using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using TriviaApp.Contexts;
using TriviaApp.Models;
namespace TriviaApp;

public static class TriviaQuestionEndpoints
{
    public static void MapTriviaQuestionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/TriviaQuestion").WithTags(nameof(TriviaQuestion));

        group.MapGet("/", async (TriviaContext db) =>
        {
            return await db.triviaQuestions.ToListAsync();
        })
        .WithName("GetAllTriviaQuestions")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<TriviaQuestion>, NotFound>> (Guid id, TriviaContext db) =>
        {
            return await db.triviaQuestions.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is TriviaQuestion model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTriviaQuestionById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (Guid id, TriviaQuestion triviaQuestion, TriviaContext db) =>
        {
            var affected = await db.triviaQuestions
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, triviaQuestion.Id)
                    .SetProperty(m => m.QuestionHeader, triviaQuestion.QuestionHeader)
                    .SetProperty(m => m.QuestionDescription, triviaQuestion.QuestionDescription)
                    .SetProperty(m => m.QuestionCategory, triviaQuestion.QuestionCategory)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTriviaQuestion")
        .WithOpenApi();

        group.MapPost("/", async (TriviaQuestion triviaQuestion, TriviaContext db) =>
        {
            db.triviaQuestions.Add(triviaQuestion);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/TriviaQuestion/{triviaQuestion.Id}",triviaQuestion);
        })
        .WithName("CreateTriviaQuestion")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, TriviaContext db) =>
        {
            var affected = await db.triviaQuestions
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTriviaQuestion")
        .WithOpenApi();
    }
}
