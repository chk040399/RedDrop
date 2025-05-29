namespace BD.PublicPortal.Api.Kafka;


public class KafkaConsumerOptions
{
  public string[] ConsumerTopics { get; set; } = [];
  public Dictionary<string, Type> KeyToEventType { get; set; } = new();

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

    // 
    //
    //services.Configure<KafkaConsumerOptions>(o => { o.ConsumerTopics =  ["NewBloodRequests", "PledgesUpdates", "DonnorsUpdate"]; });

    builder.Services.PostConfigure<KafkaConsumerOptions>(config =>
    {
      {
        config.ConsumerTopics = ["Poztzt", "zzzz"];
      //config.KeyToEventType = new Dictionary<string, Type>
      //{
      //  { "BloodRequestCreated", typeof() },
      //  { "BloodRequestStatusUpdated", typeof() },
      //  { "PledgeStatusUpdated", typeof() },
      //  { "CtsCreated", typeof() }
      };
    });

    services.AddHostedService<KafkaConsumerBackgroundService<string, string>>();

    return services;
  }


}
