
using BD.PublicPortal.Infrastructure.Data;
using BD.PublicPortal.Infrastructure.Interfaces.Database;

namespace BD.PublicPortal.Application.Database;

public class MigrateDatabaseHandler(IDatabaseManagementService databaseManagementService, AppDbContext dbContext) : ICommandHandler<MigrateDatabaseCommand,Result>
{
  public async Task<Result> Handle(MigrateDatabaseCommand request, CancellationToken cancellationToken)
  {
      return await databaseManagementService.ExecuteDbUp(dbContext, cancellationToken);
  }
}
