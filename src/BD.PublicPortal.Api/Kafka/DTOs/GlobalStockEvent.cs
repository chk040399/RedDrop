using BD.PublicPortal.Api.CtsModel.ValueObjects;
using BD.PublicPortal.Core.Entities.Enums;

namespace BD.PublicPortal.Api.Kafka.EventDTOs;

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
