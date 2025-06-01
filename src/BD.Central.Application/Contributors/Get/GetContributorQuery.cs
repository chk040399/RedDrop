using BD.Central.Infrastructure.Services.Contibutors;
using BD.SharedKernel;

namespace BD.Central.Application.Contributors.Get;

public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
