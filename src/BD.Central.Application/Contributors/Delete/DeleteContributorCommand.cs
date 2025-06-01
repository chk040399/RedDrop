using BD.SharedKernel;

namespace BD.Central.Application.Contributors.Delete;

public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
