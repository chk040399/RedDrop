// Infrastructure/Configuration/KafkaSettings.cs
namespace Infrastructure.ExternalServices.Kafka
{
    public class KafkaSettings
    {
        public const string SectionName = "Kafka";
        
        //public required string BootstrapServers { get; set; } = "kafka-broker:9092";
        public required Dictionary<string, string> Topics { get; set; }
        public List<string> ConsumerTopics { get; set; } = new();

    }
}
