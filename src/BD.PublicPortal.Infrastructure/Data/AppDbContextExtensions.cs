namespace BD.PublicPortal.Infrastructure.Data;

using Npgsql.EntityFrameworkCore.PostgreSQL;

public static class AppDbContextExtensions
{
  public static void AddApplicationDbContext(this IServiceCollection services, string connectionString) =>
    services.AddDbContext<AppDbContext>(options =>
         options.UseNpgsql(connectionString));

}
