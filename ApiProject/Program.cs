using ApiProject.Data;
using ApiProject.Data.AppContextFile;
using ApiProject.Extensions;
using ApiProject.Middleware;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllers();

/* Dependency injection and DBcontext Extension Methods. */
builder.Services.AddApplicationServices(builder.Configuration);
/* Authentication Extension Methods. */
builder.Services.AddIdentityServices(builder.Configuration);



var app = builder.Build();

//configure the HTTP request pipeline
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors( builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseAuthentication(); // check if you have valid jwt token
app.UseAuthorization();  // check if you have token given by this server

app.MapControllers();

//seeding data.
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try{
    var context = services.GetRequiredService<DatingAppContext>();
    await context.Database.MigrateAsync(); //will apply pending migration or create whole database if its is missing.
    await SeedData.SeedUser(context);
}
catch(Exception ex){
 var logger =  services.GetService<ILogger<Program>>();
 logger.LogError(ex,"An error occurred during migration");
}

app.Run();
