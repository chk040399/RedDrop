using System.Text;
using BD.PublicPortal.Core;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Interfaces.Contributors;
using BD.PublicPortal.Core.Services.Contibutors;
using BD.PublicPortal.Infrastructure.Data;
using BD.PublicPortal.Infrastructure.Data.Services;
using BD.PublicPortal.Infrastructure.Interfaces.Database;
using BD.PublicPortal.Infrastructure.Interfaces.Identity;
using BD.PublicPortal.Infrastructure.Services.Contibutors;
using BD.PublicPortal.Infrastructure.Services.Database;
using BD.PublicPortal.Infrastructure.Services.Identity;
using BD.SharedKernel;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

#nullable disable



namespace BD.PublicPortal.Infrastructure.Extensions;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    WebApplicationBuilder builder,
    ILogger logger)
  {

    var databaseName = builder.Configuration["DatabaseName"];
    if (databaseName is null) throw new Exception("Database name not indicated in the env vars");
    builder.AddNpgsqlDbContext<AppDbContext>(connectionName: databaseName, configureDbContextOptions:
      options =>
      {
        options.EnableSensitiveDataLogging().EnableDetailedErrors();

      }
    );

    // Register Identity with custom user and role
    services.AddIdentity<ApplicationUser, ApplicationRole>()
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders(); // .AddSignInManager(); // Add this if you need SignInManager


    services.AddScoped<AppDbContext>();

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
           .AddScoped<IDeleteContributorService, DeleteContributorService>();

    services.AddScoped<IUserManagementService, UserManagementService>();
    services.AddScoped<IDatabaseManagementService, DatabaseManagementService>();

    var assembly = Assembly.GetAssembly(typeof(IAssemblyMarquer));
    EnumHelper.RegisterAllEnums(assembly!, "BD.PublicPortal.Core.Entities.Enums");


    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
