using Ardalis.Result;
using BD.PublicPortal.Infrastructure.Data;
using BD.PublicPortal.Infrastructure.Interfaces.Database;
using DbUp;
using Microsoft.EntityFrameworkCore;

namespace BD.PublicPortal.Infrastructure.Services.Database;

public class DatabaseManagementService : IDatabaseManagementService
{
  public Task ExecuteApiDbInitializer(AppDbContext dbContext, CancellationToken cancellationToken)
  {
    //TODO : Use result pattern
    return ApiDbInitializer.ExecuteAsync(dbContext, cancellationToken);
  }

  public Task<Result> ExecuteDbUp(AppDbContext dbContext, CancellationToken cancellationToken)
  {
    var connectionString = dbContext.Database.GetDbConnection().ConnectionString;

    EnsureDatabase.For.PostgresqlDatabase(connectionString);

    var upgrader =
      DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetAssembly(typeof(IAssemblyMarquer)))
        .WithVariablesDisabled()//necessaire pour certaines requetes
        .LogToConsole()
        .Build();

    var dbUpgrResult = upgrader.PerformUpgrade();

    return Task.FromResult(!dbUpgrResult.Successful ? Result.Error(dbUpgrResult.Error.Message) : Result.SuccessWithMessage("Upgrade Done"));
  }


}
