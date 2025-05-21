using BD.SharedKernel;

namespace BD.PublicPortal.Application.BTC;

public record ListBloodTansfusionCentersQuery(int? WilayaId):IQuery<Result<IEnumerable<BloodTansfusionCenterDTO>>>;
