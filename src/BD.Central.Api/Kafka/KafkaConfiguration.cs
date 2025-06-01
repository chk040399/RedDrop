// Infrastructure/Configuration/KafkaSettings.cs
namespace BD.Central.Api.Kafka;

  public class KafkaSettings
  {
      public const string SectionName = "Kafka";
      public required Dictionary<string, string> Topics { get; set; }
      public List<string> ConsumerTopics { get; set; } = new();

  }
