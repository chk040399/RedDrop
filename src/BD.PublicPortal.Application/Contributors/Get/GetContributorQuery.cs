using BD.PublicPortal.Infrastructure.Services.Contibutors;

namespace BD.PublicPortal.Application.Contributors.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
