using System.Reflection;
using Application.Features.EventHandling.Commands;
using Application.Interfaces;
using Domain.Repositories;
using HSTS_Back;
using Infrastructure.Configuration;
using Infrastructure.ExternalServices;
using Infrastructure.ExternalServices.Kafka;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.DependencyInjection
{
        public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register core repositories
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBloodBagRepository, BloodBagRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IDonorRepository, DonorRepository>();
            services.AddScoped<IGlobalStockRepository, GlobalStockRepository>();
            services.AddScoped<IPledgeRepository, PledgeRepository>();
            // Add to your service registration section
            services.AddScoped<IBloodTransferCenterRepository, BloodTransferCenterRepository>();
            services.AddScoped<ICommuneRepository, CommuneRepository>();
            services.AddScoped<IWilayaRepository, WilayaRepository>();
            // Configure Kafka services
            services.AddKafkaServices(configuration);

            // Add these in your ConfigureServices method
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IPushSubscriptionRepository, PushSubscriptionRepository>();
            services.AddScoped<IWebPushService, WebPushService>();
            services.AddScoped<INotificationService, NotificationService>();

            // Configure WebPush options
            services.Configure<WebPushOptions>(configuration.GetSection("WebPush"));

            return services;
        }

        private static IServiceCollection AddKafkaServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Bind Kafka configuration
            services.Configure<KafkaSettings>(
                configuration.GetSection(KafkaSettings.SectionName));
            ;

       services.AddSingleton<ITopicDispatcher, TopicDispatcher>();
      
       // Kafka infrastructure components
       //TODO : Disabled not needed
      //services.AddSingleton<KafkaTopicInitializer>();
      services.AddScoped<IEventProducer, KafkaEventPublisher>();
      services.AddHostedService<KafkaConsumerService>();

      // TODO : Disabled temp
      /*
    // Health check registration
      services.AddHealthChecks()
          .AddKafka(new Confluent.Kafka.ProducerConfig
          {
            BootstrapServers = configuration.GetSection("Kafka:BootstrapServers").Value
          })
          .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!);
      */

        return services;
        }
        
    }
}
