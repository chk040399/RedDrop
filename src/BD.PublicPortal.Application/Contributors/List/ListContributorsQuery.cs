using BD.PublicPortal.Infrastructure.Services.Contibutors;

namespace BD.PublicPortal.Application.Contributors.List;

public record ListContributorsQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<ContributorDTO>>>;
