namespace BD.Central.Api.Kafka;

  public interface IEventProducer
  {
      Task ProduceAsync<TEvent>(string topic, TEvent @event) where TEvent : class;
      Task FlushAsync(TimeSpan timeout);
  }