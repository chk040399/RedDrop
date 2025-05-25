using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Application.Communes;

public record ListCommunesQuery(int? WilayaId) :IQuery<Result<IEnumerable<CommuneDTO>>>;
