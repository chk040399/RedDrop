using System.Text;
using BD.PublicPortal.Api.Configurations;
using BD.PublicPortal.Api.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();


builder.Services.AddCors(o => o.AddPolicy("AllowAll", o =>
{
  o.AllowAnyOrigin();
  o.AllowAnyHeader();
  o.AllowAnyMethod();
}));

builder.AddLoggerConfigs();

var appLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<BD.PublicPortal.Api.Program>();

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
  .SwaggerDocument(o =>
  {
    o.ShortSchemaNames = true;
    o.DocumentSettings = s =>
    {
      s.Title = "RedDrop WebPortal API";
      s.Version = "v1";
      s.Description = "RedDrop WebPortal API";
    };
  });




#if DEBUG
builder.WebHost.ConfigureKestrel(options =>
{
  options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
  options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(30);
});
#endif


var app = builder.Build();

// TODO : Disabled for now
// Move logging middleware to be one of the first middleware components
// Custom request logging middleware
//app.Use(async (context, next) => {
//  var start = DateTime.UtcNow;
//  var requestPath = context.Request.Path;
//  var method = context.Request.Method;

//  app.Logger.LogInformation("Request started: {Method} {Path}", method, requestPath);

//  try
//  {
//    await next();

//    var elapsed = DateTime.UtcNow - start;
//    var statusCode = context.Response.StatusCode;

//    app.Logger.LogInformation(
//      "Request completed: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms",
//      method, requestPath, statusCode, elapsed.TotalMilliseconds);
//  }
//  catch (Exception ex)
//  {
//    app.Logger.LogError(ex, "Request failed: {Method} {Path}", method, requestPath);
//    throw; // Re-throw to let the exception handler middleware handle it
//  }
//});

// Basic health check endpoint for troubleshooting
app.MapGet("/", () => "Hello from WebPortal API!");
app.MapGet("/system/debug", (HttpContext context) => {
  return $"Debug info - Remote IP: {context.Connection.RemoteIpAddress}";
});
app.MapHealthChecks("/health");
app.MapGet("/system/ping", () => Results.Ok("pong"));
app.MapGet("/system/status", () => new {
  Status = "Running",
  SchedulerActive = true,
  CurrentTime = DateTime.Now
});



await app.UseAppMiddlewareAndSeedDatabase();

//topics
var topicDispatcher = app.Services.GetRequiredService<ITopicDispatcher>();
topicDispatcher.Register<>("cts-init");
topicDispatcher.Register<>("blood-request-created");
topicDispatcher.Register<>("update-request");
topicDispatcher.Register<>("pledge-canceled-events");



logger.Information("Starting web host");
app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
namespace BD.PublicPortal.Api
{
  public partial class Program { }
}
