using System.Reflection;

namespace BD.Central.Api.Configurations;

public static class MediatrConfigs
{
  public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
  {
    var mediatRAssemblies = new[]
    {
      Assembly.GetAssembly(typeof(Central.Core.IAssemblyMarquer)), // Core
      Assembly.GetAssembly(typeof(BD.Central.Application.IAssemblyMarquer)), // App
      Assembly.GetAssembly(typeof(BD.Central.Api.IAssemblyMarquer)) // Web,


    };

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
      .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
      .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

    return services;
  }
}
