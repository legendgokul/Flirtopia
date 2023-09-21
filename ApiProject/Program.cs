using ApiProject.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllers();

var connection = builder.Configuration.GetSection("DbConnection").Value;
builder.Services.AddDbContext<DatingAppContext>(opts => opts.UseNpgsql(connection));

//Adding cors
builder.Services.AddCors();

var app = builder.Build();


app.UseCors( builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.MapControllers();

app.Run();
