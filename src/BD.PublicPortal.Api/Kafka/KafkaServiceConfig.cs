namespace BD.PublicPortal.Api.Kafka;



public static class KafkaServiceConfig
{
  public static IServiceCollection AddKafkaServiceConfigs(this IServiceCollection services,WebApplicationBuilder builder)
  {

    // Bind Kafka configuration
    services.Configure<KafkaSettings>(
      builder.Configuration.GetSection(KafkaSettings.SectionName));

    builder.AddKafkaProducer<string, string>("kafka", o => { o.Config.AllowAutoCreateTopics = true; });

    builder.AddKafkaConsumer<string, string>("kafka", o => {
      o.Config.GroupId = "RedDropWebPortal";
      o.Config.AllowAutoCreateTopics = true;
    });


    services.AddSingleton<ITopicDispatcher, TopicDispatcher>();

    // Kafka infrastructure components
    //TODO : Disabled not needed
    //services.AddSingleton<KafkaTopicInitializer>();
    services.AddScoped<IEventProducer, KafkaEventPublisher>();
    services.AddHostedService<KafkaConsumerBackgroundService>();
    return services;
  }


}
