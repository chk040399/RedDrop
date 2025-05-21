using BD.PublicPortal.Infrastructure.Services.Contibutors;
using BD.SharedKernel;

namespace BD.PublicPortal.Application.Contributors.Update;

public record UpdateContributorCommand(int ContributorId, string NewName) : ICommand<Result<ContributorDTO>>;
