using Ardalis.Result;
using BD.PublicPortal.Infrastructure.Data;

namespace BD.PublicPortal.Infrastructure.Interfaces.Database;

public interface IDatabaseManagementService
{
  public Task ExecuteApiDbInitializer(AppDbContext dbContext, CancellationToken cancellationToken);
  public Task<Result> ExecuteDbUp(AppDbContext dbContext, CancellationToken cancellationToken);
}
