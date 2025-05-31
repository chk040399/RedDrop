using BD.PublicPortal.Api.CtsModel.ValueObjects;

namespace BD.PublicPortal.Api.Kafka.EventDTOs;

  public sealed record GlobalStockEvent(
      Guid HospitalId,
      GlobalStockData GlobalStockData
  );
  public sealed record GlobalStockData(
      BloodType BloodgGroup,
      BloodBagType BloodBagType,
      int Quantity,
      int ReadyCount,
      int MinStock,
      int ExpiredCount
  );
