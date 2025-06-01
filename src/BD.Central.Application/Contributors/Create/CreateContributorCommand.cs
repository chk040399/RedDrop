using BD.SharedKernel;

namespace BD.Central.Application.Contributors.Create;

/// <summary>
/// Create a new Contributor.
/// </summary>
/// <param name="Name"></param>
public record CreateContributorCommand(string Name, string? PhoneNumber) : ICommand<Result<int>>;
