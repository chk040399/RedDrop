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
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                logger.LogInformation("Running database migrations...");
                await dbContext.Database.MigrateAsync();

                // Fix: Load all users first and then check in memory
                var anyAdminExists = await dbContext.Users
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

                    dbContext.Users.Add(adminUser);
                    await dbContext.SaveChangesAsync();
                    logger.LogInformation("Admin user seeded successfully");
                }

                // Additional seeding can be added here
                var anyWilayasExist = await dbContext.Wilayas.AsNoTracking().AnyAsync();
                if (!anyWilayasExist)
                {
                  dbContext.Wilayas.Add(new Wilaya(1, "Adrar"));
                  dbContext.Wilayas.Add(new Wilaya(2, "Chlef"));
                  dbContext.Wilayas.Add(new Wilaya(3, "Laghouat"));
                  dbContext.Wilayas.Add(new Wilaya(4, "Oum El Bouaghi"));
                  dbContext.Wilayas.Add(new Wilaya(5, "Batna"));
                  dbContext.Wilayas.Add(new Wilaya(6, "Béjaïa"));
                  dbContext.Wilayas.Add(new Wilaya(7, "Biskra"));
                  dbContext.Wilayas.Add(new Wilaya(8, "Béchar"));
                  dbContext.Wilayas.Add(new Wilaya(9, "Blida"));
                  dbContext.Wilayas.Add(new Wilaya(10, "Bouira"));
                  dbContext.Wilayas.Add(new Wilaya(11, "Tamanrasset"));
                  dbContext.Wilayas.Add(new Wilaya(12, "Tébessa"));
                  dbContext.Wilayas.Add(new Wilaya(13, "Tlemcen"));
                  dbContext.Wilayas.Add(new Wilaya(14, "Tiaret"));
                  dbContext.Wilayas.Add(new Wilaya(15, "Tizi Ouzou"));
                  dbContext.Wilayas.Add(new Wilaya(16, "Alger"));
                  dbContext.Wilayas.Add(new Wilaya(17, "Djelfa"));
                  dbContext.Wilayas.Add(new Wilaya(18, "Jijel"));
                  dbContext.Wilayas.Add(new Wilaya(19, "Sétif"));
                  dbContext.Wilayas.Add(new Wilaya(20, "Saïda"));
                  dbContext.Wilayas.Add(new Wilaya(21, "Skikda"));
                  dbContext.Wilayas.Add(new Wilaya(22, "Sidi Bel Abbès"));
                  dbContext.Wilayas.Add(new Wilaya(23, "Annaba"));
                  dbContext.Wilayas.Add(new Wilaya(24, "Guelma"));
                  dbContext.Wilayas.Add(new Wilaya(25, "Constantine"));
                  dbContext.Wilayas.Add(new Wilaya(26, "Médéa"));
                  dbContext.Wilayas.Add(new Wilaya(27, "Mostaganem"));
                  dbContext.Wilayas.Add(new Wilaya(28, "M'Sila"));
                  dbContext.Wilayas.Add(new Wilaya(29, "Mascara"));
                  dbContext.Wilayas.Add(new Wilaya(30, "Ouargla"));
                  dbContext.Wilayas.Add(new Wilaya(31, "Oran"));
                  dbContext.Wilayas.Add(new Wilaya(32, "El Bayadh"));
                  dbContext.Wilayas.Add(new Wilaya(33, "Illizi"));
                  dbContext.Wilayas.Add(new Wilaya(34, "Bordj Bou Arreridj"));
                  dbContext.Wilayas.Add(new Wilaya(35, "Boumerdès"));
                  dbContext.Wilayas.Add(new Wilaya(36, "El Tarf"));
                  dbContext.Wilayas.Add(new Wilaya(37, "Tindouf"));
                  dbContext.Wilayas.Add(new Wilaya(38, "Tissemsilt"));
                  dbContext.Wilayas.Add(new Wilaya(39, "El Oued"));
                  dbContext.Wilayas.Add(new Wilaya(40, "Khenchela"));
                  dbContext.Wilayas.Add(new Wilaya(41, "Souk Ahras"));
                  dbContext.Wilayas.Add(new Wilaya(42, "Tipaza"));
                  dbContext.Wilayas.Add(new Wilaya(43, "Mila"));
                  dbContext.Wilayas.Add(new Wilaya(44, "Aïn Defla"));
                  dbContext.Wilayas.Add(new Wilaya(45, "Naâma"));
                  dbContext.Wilayas.Add(new Wilaya(46, "Aïn Témouchent"));
                  dbContext.Wilayas.Add(new Wilaya(47, "Ghardaïa"));
                  dbContext.Wilayas.Add(new Wilaya(48, "Relizane"));
                  dbContext.Wilayas.Add(new Wilaya(49, "Timimoun"));
                  dbContext.Wilayas.Add(new Wilaya(50, "Bordj Badji Mokhtar"));
                  dbContext.Wilayas.Add(new Wilaya(51, "Ouled Djellal"));
                  dbContext.Wilayas.Add(new Wilaya(52, "Béni Abbès"));
                  dbContext.Wilayas.Add(new Wilaya(53, "In Salah"));
                  dbContext.Wilayas.Add(new Wilaya(54, "In Guezzam"));
                  dbContext.Wilayas.Add(new Wilaya(55, "Touggourt"));
                  dbContext.Wilayas.Add(new Wilaya(56, "Djanet"));
                  dbContext.Wilayas.Add(new Wilaya(57, "El Meghaier"));
                  dbContext.Wilayas.Add(new Wilaya(58, "El Menia"));
                  await dbContext.SaveChangesAsync();
                  logger.LogInformation("Wilayas seeded successfully");
        }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database seeding");
                throw;
            }
        }
    }
}
