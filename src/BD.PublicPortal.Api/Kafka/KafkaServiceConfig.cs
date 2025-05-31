namespace BD.PublicPortal.Api.Kafka;



public static class KafkaServiceConfig
{
  public static IServiceCollection AddKafkaServiceConfigs(this IServiceCollection services,WebApplicationBuilder builder)
  {

    // Bind Kafka configuration
    services.Configure<KafkaSettings>(
      builder.Configuration.GetSection(KafkaSettings.SectionName));
    

    services.AddSingleton<ITopicDispatcher, TopicDispatcher>();

    // Kafka infrastructure components
    //TODO : Disabled not needed
    //services.AddSingleton<KafkaTopicInitializer>();
    services.AddScoped<IEventProducer, KafkaEventPublisher>();
    services.AddHostedService<KafkaConsumerService>();
    return services;
  }


}
