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
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("DatabaseSeeder");
                
                // Check if database exists
                bool dbExists = await db.Database.CanConnectAsync();
                
                if (!dbExists)
                {
                    logger.LogInformation("Database does not exist. Creating database...");
                    await db.Database.EnsureCreatedAsync();
                }
                else
                {
                    // Check if tables exist by trying a simple query
                    try
                    {
                        // Try a simple query to check if tables exist
                        await db.Database.ExecuteSqlRawAsync("SELECT 1 FROM \"Users\" LIMIT 1");
                        logger.LogInformation("Database schema verified.");
                    }
                    catch (Npgsql.PostgresException ex) when (ex.SqlState == "42P01") // 42P01 = relation does not exist
                    {
                        // Tables don't exist, create them
                        logger.LogWarning("Tables don't exist. Creating database schema...");
                        await db.Database.EnsureCreatedAsync();
                    }
                }
                
                // Continue with seeding (after tables exist)
                await SeedUsers(db, logger);
                await SeedWilayas(db, logger);
                
                logger.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error during database seeding: {ex.Message}");
                throw;
            }
        }

        private static async Task SeedUsers(ApplicationDbContext db, ILogger logger)
        {
            // Check if any admin exists - using a simpler query that can be translated
            bool anyAdminExists = false;
            try
            {
                // Use string comparison instead of value object comparison
                anyAdminExists = await db.Users.AnyAsync(u => u.Email == "admin@hsts.com");
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Error checking for admin users: {ex.Message}");
            }

            if (!anyAdminExists)
            {
                logger.LogInformation("Seeding admin user...");
                try
                {
                    var adminUser = new User(
                        name: "Admin",
                        email: "admin@hsts.com",
                        password: "Admin123!",
                        role: UserRole.Admin(),
                        dateOfBirth: new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        phoneNumber: "0000000000",
                        address: "Admin Address"
                    );

                    db.Users.Add(adminUser);
                    await db.SaveChangesAsync();
                    logger.LogInformation("Admin user seeded successfully");
                }
                catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException npgEx && npgEx.SqlState == "23505")
                {
                    // Handle the case where admin already exists (unique constraint violation)
                    logger.LogInformation("Admin user already exists (detected duplicate key)");
                }
            }
        }

        private static async Task SeedWilayas(ApplicationDbContext db, ILogger logger)
        {
            // Check if any wilayas exist
            bool anyWilayasExist = false;
            try
            {
                anyWilayasExist = await db.Wilayas.AnyAsync();
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Error checking for wilayas: {ex.Message}");
            }

            if (!anyWilayasExist)
            {
                // Add all wilayas (your existing code)
                db.Wilayas.Add(new Wilaya(1, "Adrar"));
                db.Wilayas.Add(new Wilaya(2, "Chlef"));
                db.Wilayas.Add(new Wilaya(3, "Laghouat"));
                db.Wilayas.Add(new Wilaya(4, "Oum El Bouaghi"));
                db.Wilayas.Add(new Wilaya(5, "Batna"));
                db.Wilayas.Add(new Wilaya(6, "Béjaïa"));
                db.Wilayas.Add(new Wilaya(7, "Biskra"));
                db.Wilayas.Add(new Wilaya(8, "Béchar"));
                db.Wilayas.Add(new Wilaya(9, "Blida"));
                db.Wilayas.Add(new Wilaya(10, "Bouira"));
                db.Wilayas.Add(new Wilaya(11, "Tamanrasset"));
                db.Wilayas.Add(new Wilaya(12, "Tébessa"));
                db.Wilayas.Add(new Wilaya(13, "Tlemcen"));
                db.Wilayas.Add(new Wilaya(14, "Tiaret"));
                db.Wilayas.Add(new Wilaya(15, "Tizi Ouzou"));
                db.Wilayas.Add(new Wilaya(16, "Alger"));
                db.Wilayas.Add(new Wilaya(17, "Djelfa"));
                db.Wilayas.Add(new Wilaya(18, "Jijel"));
                db.Wilayas.Add(new Wilaya(19, "Sétif"));
                db.Wilayas.Add(new Wilaya(20, "Saïda"));
                db.Wilayas.Add(new Wilaya(21, "Skikda"));
                db.Wilayas.Add(new Wilaya(22, "Sidi Bel Abbès"));
                db.Wilayas.Add(new Wilaya(23, "Annaba"));
                db.Wilayas.Add(new Wilaya(24, "Guelma"));
                db.Wilayas.Add(new Wilaya(25, "Constantine"));
                db.Wilayas.Add(new Wilaya(26, "Médéa"));
                db.Wilayas.Add(new Wilaya(27, "Mostaganem"));
                db.Wilayas.Add(new Wilaya(28, "M'Sila"));
                db.Wilayas.Add(new Wilaya(29, "Mascara"));
                db.Wilayas.Add(new Wilaya(30, "Ouargla"));
                db.Wilayas.Add(new Wilaya(31, "Oran"));
                db.Wilayas.Add(new Wilaya(32, "El Bayadh"));
                db.Wilayas.Add(new Wilaya(33, "Illizi"));
                db.Wilayas.Add(new Wilaya(34, "Bordj Bou Arreridj"));
                db.Wilayas.Add(new Wilaya(35, "Boumerdès"));
                db.Wilayas.Add(new Wilaya(36, "El Tarf"));
                db.Wilayas.Add(new Wilaya(37, "Tindouf"));
                db.Wilayas.Add(new Wilaya(38, "Tissemsilt"));
                db.Wilayas.Add(new Wilaya(39, "El Oued"));
                db.Wilayas.Add(new Wilaya(40, "Khenchela"));
                db.Wilayas.Add(new Wilaya(41, "Souk Ahras"));
                db.Wilayas.Add(new Wilaya(42, "Tipaza"));
                db.Wilayas.Add(new Wilaya(43, "Mila"));
                db.Wilayas.Add(new Wilaya(44, "Aïn Defla"));
                db.Wilayas.Add(new Wilaya(45, "Naâma"));
                db.Wilayas.Add(new Wilaya(46, "Aïn Témouchent"));
                db.Wilayas.Add(new Wilaya(47, "Ghardaïa"));
                db.Wilayas.Add(new Wilaya(48, "Relizane"));
                db.Wilayas.Add(new Wilaya(49, "Timimoun"));
                db.Wilayas.Add(new Wilaya(50, "Bordj Badji Mokhtar"));
                db.Wilayas.Add(new Wilaya(51, "Ouled Djellal"));
                db.Wilayas.Add(new Wilaya(52, "Béni Abbès"));
                db.Wilayas.Add(new Wilaya(53, "In Salah"));
                db.Wilayas.Add(new Wilaya(54, "In Guezzam"));
                db.Wilayas.Add(new Wilaya(55, "Touggourt"));
                db.Wilayas.Add(new Wilaya(56, "Djanet"));
                db.Wilayas.Add(new Wilaya(57, "El Meghaier"));
                db.Wilayas.Add(new Wilaya(58, "El Menia"));
                await db.SaveChangesAsync();
                logger.LogInformation("Wilayas seeded successfully");
            }
        }
    }
}
