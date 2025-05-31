using System;

namespace BD.PublicPortal.Api.Kafka.Events;

  /// <summary>
  /// Event raised when a pledge is canceled.
  /// </summary>
  /// <param name="HospitalId">The ID of the hospital.</param>
  /// <param name="DonorId">The ID of the donor.</param>
  /// <param name="RequestId">The ID of the request.</param>
  /// <param name="PledgeDate">The date of the pledge.</param>
  public record PledgeCanceledEvent(
      Guid HospitalId,
      Guid DonorId,
      Guid RequestId,
      DateTime? PledgeDate
  );