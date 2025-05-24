using BD.SharedKernel;

namespace BD.PublicPortal.Application.Communes;

public record ListCommunesQuery(int? WilayaId):IQuery<Result<IEnumerable<CommuneDTO>>>;
