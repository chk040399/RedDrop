using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.BTC;

public record ListBloodTansfusionCentersQuery(BloodTransfusionCenterSpecificationFilter? filter = null, int? Level = null)
  :IQuery<Result<IEnumerable<BloodTansfusionCenterExDTO>>>;
