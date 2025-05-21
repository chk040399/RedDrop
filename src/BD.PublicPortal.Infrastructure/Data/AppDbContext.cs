using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.Entities.Contributors;
using BD.SharedKernel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


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
        var entitiesWithEvents = ChangeTracker.Entries<HasDomainEventsBase>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return result;
    }

    public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}
