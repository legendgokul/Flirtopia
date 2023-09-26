using ApiProject.BusinessLayer.Interface;
using ApiProject.BusinessLayer.Service;
using ApiProject.Data.AppContextFile;
using Microsoft.EntityFrameworkCore;

namespace ApiProject.Extensions;

//Use the class and its methods directly instead of  creating an instance .
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config){

        var connection = config.GetSection("DbConnection").Value;
        services.AddDbContext<DatingAppContext>(opts => opts.UseNpgsql(connection));

        //Adding cors
        services.AddCors();
        services.AddScoped<ITokenService,TokenService>();
        return services;

    }
}
