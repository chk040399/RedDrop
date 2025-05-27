
using BD.PublicPortal.Infrastructure.Data;
using BD.PublicPortal.Infrastructure.Interfaces.Database;

namespace BD.PublicPortal.Application.Database;

public class MigrateDatabaseHandler(IDatabaseManagementService databaseManagementService, AppDbContext dbContext) : ICommandHandler<MigrateDatabaseCommand,Result>
{
  public async Task<Result> Handle(MigrateDatabaseCommand request, CancellationToken cancellationToken)
  {
    try
    {
      await databaseManagementService.ExecuteAsync(dbContext, cancellationToken);
      return Result.SuccessWithMessage("Database successfully migrated");
    }
    catch (Exception e)
    {
      return Result.Error(e.Message);
    }
  }
}
