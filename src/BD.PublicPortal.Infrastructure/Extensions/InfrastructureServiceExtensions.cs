using System.Text;
using BD.PublicPortal.Core;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Interfaces.Contributors;
using BD.PublicPortal.Core.Services.Contibutors;
using BD.PublicPortal.Infrastructure.Data;
using BD.PublicPortal.Infrastructure.Data.Services;
using BD.PublicPortal.Infrastructure.Interfaces.Identity;
using BD.PublicPortal.Infrastructure.Services.Contibutors;
using BD.PublicPortal.Infrastructure.Services.Identity;
using BD.SharedKernel;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

#nullable disable



namespace BD.PublicPortal.Infrastructure.Extensions;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    var connectionString = config.GetConnectionString("DefaultConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
     options.UseNpgsql(connectionString).EnableSensitiveDataLogging().EnableDetailedErrors());

    // Register Identity with custom user and role
    services.AddIdentity<ApplicationUser, ApplicationRole>()
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders(); // .AddSignInManager(); // Add this if you need SignInManager

    


    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
           .AddScoped<IDeleteContributorService, DeleteContributorService>();

    services.AddScoped<IUserManagementService, UserManagementService>();

    var assembly = Assembly.GetAssembly(typeof(BD.PublicPortal.Core.IAssemblyMarquer));
    EnumHelper.RegisterAllEnums(assembly!, "BD.PublicPortal.Core.Entities.Enums");


    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
