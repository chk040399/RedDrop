using Confluent.Kafka;

namespace BD.Central.Api.Features.InternalUse;

public record TestKafkaEndpointRequest(string Topic, string Key, string Message);

public class TestKafkaEndpoint(IProducer<string,string> _producer):Endpoint<TestKafkaEndpointRequest, string>
{
  public override void Configure()
  {
    Post("internal/kafakapublish");
    AllowAnonymous();
  }

  public override async Task HandleAsync(TestKafkaEndpointRequest req,CancellationToken ct)
  {
    try
    {
      var result = await _producer.ProduceAsync(
        topic: req.Topic,
        message: new Message<string, string>
        {
          Key = req.Key,
          Value = req.Message
        });
      
      await  SendOkAsync($"Delivered to: {result.TopicPartitionOffset}",ct);
    }
    catch (ProduceException<string, string> e)
    {
      Response = e.Message;
      await SendErrorsAsync(500,ct);
    }
  }
}
