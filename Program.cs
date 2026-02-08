using Auth0.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

//using Microsoft.EntityFrameworkCore.SqlServer;
using TriviaApp;
using TriviaApp.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.Services.AddAuth0WebAppAuthentication(options =>
        //{
        //    options.Domain = builder.Configuration["Auth0:Domain"];
        //    options.ClientId = builder.Configuration["Auth0:ClientId"];
        //});

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}/",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidateLifetime = true
        };
    });
        builder.Services.AddAuthorization();

        //builder.Services.AddOpenApi();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Policy1",
                policy =>
                {
                    policy.WithOrigins("https://localhost:8000", "https://localhost:3000",
                                        "http://localhost:8000", "http://localhost:3000").AllowAnyHeader()    .AllowAnyMethod();
                });

            //options.AddPolicy("AnotherPolicy",
            //    policy =>
            //    {
            //        policy.WithOrigins("http://www.contoso.com")
            //                            .AllowAnyHeader()
            //                            .AllowAnyMethod();
            //    });
        });
        builder.Services.AddDbContext<TriviaContext>(options =>

        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))

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

        // app.UseHttpsRedirection();
        app.UseCors("Policy1");
        app.UseAuthentication();
        app.UseAuthorization();
        

        app.MapControllers();

        app.Run();
    }
}