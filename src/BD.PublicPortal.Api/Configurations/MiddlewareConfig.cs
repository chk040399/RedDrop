using Ardalis.ListStartupServices;
using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Infrastructure.Data;
using BD.PublicPortal.Infrastructure.Data.SeedData;

namespace BD.PublicPortal.Api.Configurations;

public static class MiddlewareConfig
{
  public static async Task<IApplicationBuilder> UseAppMiddlewareAndSeedDatabase(this WebApplication app)
  {
    if (app.Environment.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
      app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
    }
    else
    {
      app.UseDefaultExceptionHandler(); // from FastEndpoints
      app.UseHsts();
    }

    //app.UseFastEndpoints(x => x.Errors.UseProblemDetails())
    //    .UseSwaggerGen(); // Includes AddFileServer and static files middleware

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapGroup("/auth").WithTags("Identity").MapIdentityApi<ApplicationUser>();

    app.UseFastEndpoints(
      c =>
      {
        c.Errors.UseProblemDetails(
          x =>
          {
            x.AllowDuplicateErrors = true; //allows duplicate errors for the same error name
            x.IndicateErrorCode = true; //serializes the fluentvalidation error code
            x.IndicateErrorSeverity = true; //serializes the fluentvalidation error severity
            x.TypeValue = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1";
            x.TitleValue = "One or more validation errors occurred.";
            x.TitleTransformer = pd => pd.Status switch
            {
              400 => "Validation Error",
              404 => "Not Found",
              _ => "One or more errors occurred!"
            };
          });
      }).UseSwaggerGen(); // Includes AddFileServer and static files middleware

    app.UseHttpsRedirection(); // Note this will drop Authorization headers

    await SeedDatabase(app);

    return app;
  }

  static async Task SeedDatabase(WebApplication app)
  {
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
      var context = services.GetRequiredService<AppDbContext>();
      //          context.Database.Migrate();
      context.Database.EnsureCreated();
      await ContributorsSeedData.InitializeAsync(context);
    }
    catch (Exception ex)
    {
      var logger = services.GetRequiredService<ILogger<Program>>();
      logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
    }
  }
}
