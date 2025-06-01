using Ardalis.Result;
using BD.Central.Infrastructure.Data;

namespace BD.Central.Infrastructure.Interfaces.Database;

public interface IDatabaseManagementService
{
  public Task ExecuteApiDbInitializer(AppDbContext dbContext, CancellationToken cancellationToken);
  public Task<Result> ExecuteDbUp(AppDbContext dbContext, CancellationToken cancellationToken);
}
