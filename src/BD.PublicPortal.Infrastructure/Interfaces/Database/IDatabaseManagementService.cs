using BD.PublicPortal.Infrastructure.Data;

namespace BD.PublicPortal.Infrastructure.Interfaces.Database;

public interface IDatabaseManagementService
{
  public Task ExecuteAsync(AppDbContext dbContext, CancellationToken cancellationToken);
}
