using ApiProject.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllers();

var connection = builder.Configuration.GetSection("DbConnection").Value;
builder.Services.AddDbContext<DatingAppContext>(opts => opts.UseNpgsql(connection));

var app = builder.Build();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
