using BD.PublicPortal.Core.DTOs;

namespace BD.PublicPortal.Application.Wilayas;



public record ListWilayasQuery():IQuery<Result<IEnumerable<WilayaDTO>>>;
