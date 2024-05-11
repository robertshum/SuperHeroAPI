global using SuperHeroAPI.Data;
global using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using SuperHeroAPI.Services;

var MyAllowSpecificOrigins = "_origins";
var builder = WebApplication.CreateBuilder(args);

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:5173")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
    options.AddPolicy(MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://robertshum.github.io")
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

//add services context
builder.Services.AddScoped<IPowerService, PowerService>();
builder.Services.AddScoped<ISuperHeroService, SuperHeroService>();

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

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
