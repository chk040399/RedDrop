using FluentValidation;
using MediatR;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using HSTS_Back.Presentation.Middlewares;
using Infrastructure.Persistence;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure.DependencyInjection;
using Infrastructure.ExternalServices.Kafka;
using Application.Interfaces;
using Application.Features.EventHandling.Commands;
using Infrastructure.BackgroundServices;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HSTS_Back;
using HSTS_Back.Infrastructure.BackgroundServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Services;
var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel explicitly
//builder.WebHost.ConfigureKestrel(serverOptions => {
//    serverOptions.ListenAnyIP(5000);
//    serverOptions.AllowSynchronousIO = true;
//});

// Database
builder.AddNpgsqlDbContext<ApplicationDbContext>(connectionName: "Cts1PortalDatabase", configureDbContextOptions:
  options =>
  {
    options.EnableSensitiveDataLogging().EnableDetailedErrors();

  }
);

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
//          .EnableSensitiveDataLogging()
//          .EnableDetailedErrors());
//Console.WriteLine("Database connection string: " + builder.Configuration.GetConnectionString("DefaultConnection"));

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



// Add health checks
//builder.Services.AddHealthChecks()
//    .AddNpgSql(
//        builder.Configuration.GetConnectionString("DefaultConnection") ?? 
//        throw new InvalidOperationException("Connection string 'DefaultConnection' not found."), 
//        name: "postgres");

// Register Kafka stuff
builder.AddKafkaProducer<string, string>("kafka", o => { o.Config.AllowAutoCreateTopics = true; });

builder.AddKafkaConsumer<string, string>("kafka", o => {
  o.Config.GroupId = "CTSs";
  o.Config.AllowAutoCreateTopics = true;
});
// 
//

// TODO : exemple utilisation simple
//services.Configure<KafkaConsumerOptions>(o => { o.ConsumerTopics =  ["NewBloodRequests", "PledgesUpdates", "DonnorsUpdate"]; });

// TODO : exemple utilisation avancé
//builder.Services.PostConfigure<KafkaConsumerOptions>(config =>
//{
//  {
//    config.ConsumerTopics = ["topic1", "topic2"];
//    config.KeyToEventType = new Dictionary<string, Type>
//    {
//      { "BloodRequestCreated", typeof() },
//      { "BloodRequestStatusUpdated", typeof() },
//      { "PledgeStatusUpdated", typeof() },
//      { "CtsCreated", typeof() }
//  }
//  ;
//});

builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("Kafka"));

// Register scoped services
builder.Services.AddScoped<IEventProducer, KafkaEventPublisher>();

// Register singleton for topic initialization
//TODO : Disabled not needed
//builder.Services.AddSingleton<KafkaTopicInitializer>();

// FastEndpoints + Swagger
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "HSTS API";
        s.Version = "v1";
        s.Description = "API for HSTS project";
    };
});


//


// MediatR


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
  

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());



// Add JWT configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key")))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});


// hosted service
//TODO : Disabled Temporarly -- Renabled
builder.Services.AddHostedService<KafkaConsumerService>();


//TODO : Disabled Temporarly -- Renabled
builder.Services.AddHostedService<BackgroundEventProcessor>();
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

//TODO : Disabled Temporarly -- Renabled
// Register the DailySchedulerService
builder.Services.AddHostedService<DailySchedulerService>();


builder.AddServiceDefaults();// For ASPIRE --> Services registration

var app = builder.Build();

// Move logging middleware to be one of the first middleware components
// Custom request logging middleware
app.Use(async (context, next) => {
    var start = DateTime.UtcNow;
    var requestPath = context.Request.Path;
    var method = context.Request.Method;
    
    app.Logger.LogInformation("Request started: {Method} {Path}", method, requestPath);
    
    try {
        await next();
        
        var elapsed = DateTime.UtcNow - start;
        var statusCode = context.Response.StatusCode;
        
        app.Logger.LogInformation(
            "Request completed: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms",
            method, requestPath, statusCode, elapsed.TotalMilliseconds);
    }
    catch (Exception ex) {
        app.Logger.LogError(ex, "Request failed: {Method} {Path}", method, requestPath);
        throw; // Re-throw to let the exception handler middleware handle it
    }
});

// Basic health check endpoint for troubleshooting
app.MapGet("/", () => "Hello from HSTS API!");
app.MapGet("/debug", (HttpContext context) => {
    return $"Debug info - Remote IP: {context.Connection.RemoteIpAddress}";
});
app.MapHealthChecks("/health");
app.MapGet("/ping", () => Results.Ok("pong"));
app.MapGet("/api/system/status", () => new { 
    Status = "Running", 
    SchedulerActive = true,
    CurrentTime = DateTime.Now 
});

// Add all other middleware

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseFastEndpoints();
app.UseSwaggerGen();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");


// Register your topic handlers with the dispatcher
//TODO : Disabled
//var topicDispatcher = app.Services.GetRequiredService<ITopicDispatcher>();
//topicDispatcher.Register<DonorPledgeCommand>("donors-pledges");


app.MapDefaultEndpoints();// ASPIRE  --> Middle ware


// In Program.cs after service registration
//TODO : Disabled not needed
//var serviceProvider = app.Services;
//var topicInitializer = serviceProvider.GetRequiredService<KafkaTopicInitializer>();
//await topicInitializer.InitializeAsync();
// Add a delay to ensure topics are ready
//await Task.Delay(2000); 
//app.Logger.LogInformation("Kafka topics initialized, starting application");


// Call database seeder
await DatabaseSeeder.SeedDatabase(app.Services);

app.Run();
