using BD.PublicPortal.Infrastructure.Services.Contibutors;
using BD.SharedKernel;

namespace BD.PublicPortal.Application.Contributors.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
