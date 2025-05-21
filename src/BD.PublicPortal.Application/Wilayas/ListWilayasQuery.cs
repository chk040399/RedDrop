using BD.SharedKernel;

namespace BD.PublicPortal.Application.Wilayas;



public record ListWilayasQuery():IQuery<Result<IEnumerable<WilayaDTO>>>;
