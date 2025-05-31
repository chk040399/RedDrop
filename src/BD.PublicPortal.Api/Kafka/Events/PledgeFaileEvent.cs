namespace BD.PublicPortal.Api.Kafka.Events;

      public record PledgeFailedEvent(
      DonorPledgeEvent OriginalEvent,
      string ErrorMessage,
      DateTime FailedAt,
      Guid CorrelationId
  );