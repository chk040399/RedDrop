using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Contributors;
using BD.PublicPortal.Core.Entities.Events;
using BD.SharedKernel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace BD.PublicPortal.Infrastructure.Data;
public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? dispatcher
) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    private readonly IDomainEventDispatcher? _dispatcher = dispatcher;

  public DbSet<Contributor> Contributors => Set<Contributor>();
  public DbSet<Wilaya> Wilayas => Set<Wilaya>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    
    base.OnModelCreating(modelBuilder); // Required for Identity
       modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // ignore events if no dispatcher provided
        if (_dispatcher == null) return result;

        // dispatch events only if save was successful
        var entitiesWithEvents = ChangeTracker.Entries<IHasDomainEvents>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        // Traitement special users 
        //{
        //  var updatedUsersIds = ChangeTracker.Entries<ApplicationUser>()
        //    .Where(e => e.State == EntityState.Modified)
        //    .Select(e => e.Entity.Id).
        //    ToList();
        //  if (updatedUsersIds.Count > 0)
        //  {
        //    logger.LogInformation("!!! Found updated users (for kafka eventing)");
        //    entitiesWithEvents.Where(e => e.GetType() == typeof(ApplicationUserEvent))
        //      .Cast<ApplicationUserEvent>()
        //      .ToList()
        //      .Where(e => updatedUsersIds.Contains(e.UserEntity.Id)).ToList()
        //      .ForEach(ee => ee.EventType = "UpdatedUser");
        //  }
        //}

        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return result;
    }

    public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}
