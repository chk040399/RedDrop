using BD.Central.Infrastructure.Services.Contibutors;
using BD.SharedKernel;

namespace BD.Central.Application.Contributors.List;

public record ListContributorsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ContributorDTO>>>;
