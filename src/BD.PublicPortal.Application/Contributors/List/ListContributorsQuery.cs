using BD.PublicPortal.Infrastructure.Services.Contibutors;
using BD.SharedKernel;

namespace BD.PublicPortal.Application.Contributors.List;

public record ListContributorsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ContributorDTO>>>;
