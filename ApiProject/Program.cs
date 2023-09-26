using System.Text;
using ApiProject.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllers();

/* Dependency injection and DBcontext Extension Methods. */
builder.Services.AddApplicationServices(builder.Configuration);
/* Authentication Extension Methods. */
builder.Services.AddIdentityServices(builder.Configuration);



var app = builder.Build();




app.UseCors( builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseAuthentication(); // check if you have valid jwt token
app.UseAuthorization();  // check if you have token given by this server

app.MapControllers();


app.Run();
