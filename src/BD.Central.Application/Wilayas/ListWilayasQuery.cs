using BD.Central.Core.DTOs;

namespace BD.Central.Application.Wilayas;



public record ListWilayasQuery():IQuery<Result<IEnumerable<WilayaDTO>>>;
