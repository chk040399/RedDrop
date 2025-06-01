using BD.Central.Core.Entities.Enums;


namespace BD.Central.Api.Kafka.EventDTOs;

  public sealed record GlobalStockEvent(
      Guid HospitalId,
      GlobalStockData GlobalStockData
  );
  public sealed record GlobalStockData(
      BloodGroup BloodgGroup,
      BloodDonationType BloodBagType,
      int Quantity,
      int ReadyCount,
      int MinStock,
      int ExpiredCount
  );
