using BD.Central.Infrastructure.Services.Contibutors;
using BD.SharedKernel;

namespace BD.Central.Application.Contributors.Update;

public record UpdateContributorCommand(int ContributorId, string NewName) : ICommand<Result<ContributorDTO>>;
