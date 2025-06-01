using BD.Central.Core.DTOs;

namespace BD.Central.Application.Communes;

public record ListCommunesQuery(int? WilayaId) :IQuery<Result<IEnumerable<CommuneDTO>>>;
