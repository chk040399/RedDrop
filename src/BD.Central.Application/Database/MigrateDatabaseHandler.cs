
using BD.Central.Infrastructure.Data;
using BD.Central.Infrastructure.Interfaces.Database;

namespace BD.Central.Application.Database;

public class MigrateDatabaseHandler(IDatabaseManagementService databaseManagementService, AppDbContext dbContext) : ICommandHandler<MigrateDatabaseCommand,Result>
{
  public async Task<Result> Handle(MigrateDatabaseCommand request, CancellationToken cancellationToken)
  {
      return await databaseManagementService.ExecuteDbUp(dbContext, cancellationToken);
  }
}
