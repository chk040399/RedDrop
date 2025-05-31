using System.Reflection;

namespace BD.PublicPortal.Api.Configurations;

public static class MediatrConfigs
{
  public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
  {
    var mediatRAssemblies = new[]
    {
      Assembly.GetAssembly(typeof(BD.PublicPortal.Core.IAssemblyMarquer)), // Core
      Assembly.GetAssembly(typeof(BD.PublicPortal.Application.IAssemblyMarquer)), // App
      Assembly.GetAssembly(typeof(BD.PublicPortal.Api.IAssemblyMarquer)) // Web,


    };

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
      .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
      .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

    return services;
  }
}
