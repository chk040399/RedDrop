using System.Text;
using BD.PublicPortal.Api.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

builder.AddLoggerConfigs();

var appLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
builder.Services.AddServiceConfigs(appLogger, builder);


// Add Authentication and Authorization
{
  //services.AddAuthenticationCookie(validFor: TimeSpan.FromMinutes(60)); // Add this if you need cookie authentication

  // Configure JWT authentication
  var jwtSettings = builder.Configuration.GetSection("JwtSettings");
  var secretKey = jwtSettings["Secret"];
  if(secretKey==null)
  {
     throw new ArgumentNullException("Secret key is not configured in appsettings.json");
  }

  builder.Services.AddAuthentication(o =>
  {
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  }).AddJwtBearer(o =>
  {
    o.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = jwtSettings["Issuer"],
      ValidAudience = jwtSettings["Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
  });

  builder.Services.AddAuthorization();
}


builder.Services.AddFastEndpoints(o => o.IncludeAbstractValidators = true)
                .SwaggerDocument(o =>o.ShortSchemaNames = true);


#if DEBUG
builder.WebHost.ConfigureKestrel(options =>
{
  options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
  options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(30);
});
#endif


var app = builder.Build();



await app.UseAppMiddlewareAndSeedDatabase();



app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
