global using SuperHeroAPI.Data;
global using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using SuperHeroAPI.Services;

//Auth
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using SuperHeroAPI.MethodMeasure;

var corsPolicy = "_corsPolicy";
var builder = WebApplication.CreateBuilder(args);

//Configure JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    //Set auth to clerk's domain
    // Authority is the URL of your clerk instance
    string clerkAuthString = Environment.GetEnvironmentVariable("ClerkAuthority");

    if (string.IsNullOrEmpty(clerkAuthString))
    {
        clerkAuthString = builder.Configuration["ClerkAuthority"];
    }

    x.Authority = clerkAuthString;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        //disable audience validation if not needed
        ValidateIssuer = false,
        ValidateAudience = false,
        NameClaimType = ClaimTypes.NameIdentifier,
    };
    x.Events = new JwtBearerEvents()
    {
        OnTokenValidated = context =>
        {

            string clerkAuthPartyString = Environment.GetEnvironmentVariable("ClerkAuthorizedParty");

            if (string.IsNullOrEmpty(clerkAuthPartyString))
            {
                clerkAuthPartyString = builder.Configuration["ClerkAuthorizedParty"];
            }

            var azp = context.Principal?.FindFirstValue("azp");

            // AuthorizedParty is the base URL of your frontend.
            if (string.IsNullOrEmpty(azp) || !azp.Equals(clerkAuthPartyString))
            {
                context.Fail("AZP Claim is invalid/missing");
                Console.WriteLine("AZP Claim is invalid/missing");
            }
            Console.WriteLine("completed");
            return Task.CompletedTask;
        }
    };
});

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173", "https://robertshum.github.io")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    //this is to reduce circular references in JSON
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Add db context for sql server
builder.Services.AddDbContext<DataContext>(options =>
{
    // Load connection string from environment variable
    string connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");


    if (string.IsNullOrEmpty(connectionString))
    {
        connectionString = "Development";
        options.UseSqlServer(builder.Configuration.GetConnectionString(connectionString));
    }
    // connection s tring coming from outside
    else
    {
        options.UseSqlServer(connectionString);
    }

});

//add health check
builder.Services.AddHealthChecks();

//add services context
builder.Services.AddScoped<IPowerService, PowerService>();
builder.Services.AddScoped<ISuperHeroService, SuperHeroService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// After build of app:

//logger
MethodTimeLogger.Logger = app.Logger;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.MapHealthChecks("/healthz");

app.MapControllers();

app.Run();
