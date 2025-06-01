using BD.Central.Core.DTOs;
using BD.Central.Core.Entities.Specifications;

namespace BD.Central.Application.BTC;

public record ListBloodTansfusionCentersQuery(BloodTransfusionCenterSpecificationFilter? filter = null, int? Level = null)
  :IQuery<Result<IEnumerable<BloodTansfusionCenterExDTO>>>;
