using BD.PublicPortal.Infrastructure.Data;
using BD.PublicPortal.Infrastructure.Interfaces.Database;

namespace BD.PublicPortal.Infrastructure.Services.Database;

public class DatabaseManagementService : IDatabaseManagementService
{
  public Task ExecuteAsync(AppDbContext dbContext, CancellationToken cancellationToken)
  {
    return ApiDbInitializer.ExecuteAsync(dbContext, cancellationToken);
  }
}
