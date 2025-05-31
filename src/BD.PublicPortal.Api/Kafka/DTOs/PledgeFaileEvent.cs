namespace BD.PublicPortal.Api.Kafka.EventDTOs;

      public record PledgeFailedEvent(
      DonorPledgeEvent OriginalEvent,
      string ErrorMessage,
      DateTime FailedAt,
      Guid CorrelationId
  );
