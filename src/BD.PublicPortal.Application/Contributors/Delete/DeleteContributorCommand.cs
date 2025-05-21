using BD.SharedKernel;

namespace BD.PublicPortal.Application.Contributors.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
