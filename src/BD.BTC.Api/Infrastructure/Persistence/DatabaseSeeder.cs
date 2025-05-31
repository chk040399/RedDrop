using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                // Get logger using factory instead of generic logger
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("DatabaseSeeder");
                
                // Fix for the PostgreSQL subquery constraint issue
                try
                {
                    await context.Database.ExecuteSqlRawAsync(@"
                        ALTER TABLE ""BloodTransferCenters"" DROP CONSTRAINT IF EXISTS ""CK_SingleBloodTransferCenter"";
                        
                        DO $$ 
                        BEGIN
                            IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                                          WHERE table_name = 'BloodTransferCenters' 
                                          AND column_name = 'SingletonCheck') THEN
                                ALTER TABLE ""BloodTransferCenters"" ADD COLUMN ""SingletonCheck"" INTEGER NOT NULL DEFAULT 1;
                            END IF;
                        END $$;
                        
                        DO $$
                        BEGIN
                            IF NOT EXISTS (SELECT 1 FROM pg_indexes 
                                          WHERE tablename = 'BloodTransferCenters' 
                                          AND indexname = 'IX_BloodTransferCenters_SingletonCheck') THEN
                                CREATE UNIQUE INDEX ""IX_BloodTransferCenters_SingletonCheck"" ON ""BloodTransferCenters"" (""SingletonCheck"");
                            END IF;
                        END $$;
                    ");
                }
                catch (Exception ex)
                {
                    // Use logger instead of Console.WriteLine
                    logger.LogWarning($"Constraint fix failed: {ex.Message}");
                }
                
                // Continue with migrations
                await context.Database.MigrateAsync();

                // Fix: Load all users first and then check in memory
                var anyAdminExists = await context.Users
                    .AsNoTracking()  // For better performance since we're just reading
                    .ToListAsync()    // Get all users (this moves processing to client side)
                    .ContinueWith(t => t.Result.Any(u => u.Role.Role == UserRole.Admin().Role));

                if (!anyAdminExists)
                {
                    logger.LogInformation("Seeding admin user...");
                    var adminUser = new User(
                        name: "Admin",
                        email: "admin@hsts.com",
                        password: "Admin123!", // In a real app, this would be hashed
                        role: UserRole.Admin(),
                        dateOfBirth: new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc), // Fix: Specify UTC DateTime
                        phoneNumber: "0000000000",
                        address: "Admin Address"
                    );

                    context.Users.Add(adminUser);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Admin user seeded successfully");
                }

                // Additional seeding can be added here
                var anyWilayasExist = await context.Wilayas.AsNoTracking().AnyAsync();
                if (!anyWilayasExist)
                {
                  context.Wilayas.Add(new Wilaya(1, "Adrar"));
                  context.Wilayas.Add(new Wilaya(2, "Chlef"));
                  context.Wilayas.Add(new Wilaya(3, "Laghouat"));
                  context.Wilayas.Add(new Wilaya(4, "Oum El Bouaghi"));
                  context.Wilayas.Add(new Wilaya(5, "Batna"));
                  context.Wilayas.Add(new Wilaya(6, "Béjaïa"));
                  context.Wilayas.Add(new Wilaya(7, "Biskra"));
                  context.Wilayas.Add(new Wilaya(8, "Béchar"));
                  context.Wilayas.Add(new Wilaya(9, "Blida"));
                  context.Wilayas.Add(new Wilaya(10, "Bouira"));
                  context.Wilayas.Add(new Wilaya(11, "Tamanrasset"));
                  context.Wilayas.Add(new Wilaya(12, "Tébessa"));
                  context.Wilayas.Add(new Wilaya(13, "Tlemcen"));
                  context.Wilayas.Add(new Wilaya(14, "Tiaret"));
                  context.Wilayas.Add(new Wilaya(15, "Tizi Ouzou"));
                  context.Wilayas.Add(new Wilaya(16, "Alger"));
                  context.Wilayas.Add(new Wilaya(17, "Djelfa"));
                  context.Wilayas.Add(new Wilaya(18, "Jijel"));
                  context.Wilayas.Add(new Wilaya(19, "Sétif"));
                  context.Wilayas.Add(new Wilaya(20, "Saïda"));
                  context.Wilayas.Add(new Wilaya(21, "Skikda"));
                  context.Wilayas.Add(new Wilaya(22, "Sidi Bel Abbès"));
                  context.Wilayas.Add(new Wilaya(23, "Annaba"));
                  context.Wilayas.Add(new Wilaya(24, "Guelma"));
                  context.Wilayas.Add(new Wilaya(25, "Constantine"));
                  context.Wilayas.Add(new Wilaya(26, "Médéa"));
                  context.Wilayas.Add(new Wilaya(27, "Mostaganem"));
                  context.Wilayas.Add(new Wilaya(28, "M'Sila"));
                  context.Wilayas.Add(new Wilaya(29, "Mascara"));
                  context.Wilayas.Add(new Wilaya(30, "Ouargla"));
                  context.Wilayas.Add(new Wilaya(31, "Oran"));
                  context.Wilayas.Add(new Wilaya(32, "El Bayadh"));
                  context.Wilayas.Add(new Wilaya(33, "Illizi"));
                  context.Wilayas.Add(new Wilaya(34, "Bordj Bou Arreridj"));
                  context.Wilayas.Add(new Wilaya(35, "Boumerdès"));
                  context.Wilayas.Add(new Wilaya(36, "El Tarf"));
                  context.Wilayas.Add(new Wilaya(37, "Tindouf"));
                  context.Wilayas.Add(new Wilaya(38, "Tissemsilt"));
                  context.Wilayas.Add(new Wilaya(39, "El Oued"));
                  context.Wilayas.Add(new Wilaya(40, "Khenchela"));
                  context.Wilayas.Add(new Wilaya(41, "Souk Ahras"));
                  context.Wilayas.Add(new Wilaya(42, "Tipaza"));
                  context.Wilayas.Add(new Wilaya(43, "Mila"));
                  context.Wilayas.Add(new Wilaya(44, "Aïn Defla"));
                  context.Wilayas.Add(new Wilaya(45, "Naâma"));
                  context.Wilayas.Add(new Wilaya(46, "Aïn Témouchent"));
                  context.Wilayas.Add(new Wilaya(47, "Ghardaïa"));
                  context.Wilayas.Add(new Wilaya(48, "Relizane"));
                  context.Wilayas.Add(new Wilaya(49, "Timimoun"));
                  context.Wilayas.Add(new Wilaya(50, "Bordj Badji Mokhtar"));
                  context.Wilayas.Add(new Wilaya(51, "Ouled Djellal"));
                  context.Wilayas.Add(new Wilaya(52, "Béni Abbès"));
                  context.Wilayas.Add(new Wilaya(53, "In Salah"));
                  context.Wilayas.Add(new Wilaya(54, "In Guezzam"));
                  context.Wilayas.Add(new Wilaya(55, "Touggourt"));
                  context.Wilayas.Add(new Wilaya(56, "Djanet"));
                  context.Wilayas.Add(new Wilaya(57, "El Meghaier"));
                  context.Wilayas.Add(new Wilaya(58, "El Menia"));
                  await context.SaveChangesAsync();
                  logger.LogInformation("Wilayas seeded successfully");
        }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error during database seeding: {ex.Message}");
                throw;
            }
        }
    }
}
