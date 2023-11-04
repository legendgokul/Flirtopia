using ApiProject.BusinessLayer.Interface;
using ApiProject.BusinessLayer.Service;
using ApiProject.Data.AppContextFile;
using ApiProject.DataAccess.Interface;
using ApiProject.DataAccess.Repository;
using ApiProject.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Extensions;

//Use the class and its methods directly instead of  creating an instance .
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config){

        var connection = config.GetSection("DbConnection").Value;
        services.AddDbContext<DatingAppContext>(opts => opts.UseNpgsql(connection));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


        //Adding cors
        services.AddCors();
        services.AddScoped<ITokenService,TokenService>();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IPhotoService,PhotoService>();
        services.AddScoped<LogUserActivity>();
        
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        //binding cloudinary config into Iconfiguration.
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        return services;

    }
}
