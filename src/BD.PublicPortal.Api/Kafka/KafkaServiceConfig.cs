namespace BD.PublicPortal.Api.Kafka;


public class KafkaConsumerOptions
{
  public string[] Topics { get; set; } = [];
}

public static class KafkaServiceConfig
{
  public static IServiceCollection AddKafkaServiceConfigs(this IServiceCollection services,WebApplicationBuilder builder)
  {
    //TODO : kafka integr (temp use better integration later
    builder.AddKafkaProducer<string, string>("kafka", o => { o.Config.AllowAutoCreateTopics = true;});
    builder.AddKafkaConsumer<string, string>("kafka", o => { o.Config.GroupId = "PublicPortal";
      o.Config.AllowAutoCreateTopics = true;
    });

    services.Configure<KafkaConsumerOptions>(o => { o.Topics =  ["NewBloodRequests", "PledgesUpdates", "DonnorsUpdate"]; });

    services.AddHostedService<KafkaConsumerBackgroundService<string, string>>();

    return services;
  }


}
