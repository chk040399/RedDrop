using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class BloodTransferCenterRepository : IBloodTransferCenterRepository
    {
        private readonly ApplicationDbContext _context;
        
        public BloodTransferCenterRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<BloodTransferCenter?> GetByIdAsync(Guid id)
        {
            return await _context.BloodTransferCenters
                .Include(btc => btc.Wilaya)
                .FirstOrDefaultAsync(btc => btc.Id == id);
        }
        
        public async Task<BloodTransferCenter?> GetByNameAsync(string name)
        {
            // Using case-insensitive comparison for PostgreSQL
            return await _context.BloodTransferCenters
                .Include(btc => btc.Wilaya)
                .FirstOrDefaultAsync(btc => EF.Functions.ILike(btc.Name, name));
        }
        
        public async Task<BloodTransferCenter?> GetByEmailAsync(string email)
        {
            // Using case-insensitive comparison for PostgreSQL
            return await _context.BloodTransferCenters
                .Include(btc => btc.Wilaya)
                .FirstOrDefaultAsync(btc => EF.Functions.ILike(btc.Email, email));
        }
        
        public async Task<BloodTransferCenter?> GetPrimaryAsync()
        {
            // Just return any center since we're enforcing only one center exists
            return await GetAsync();
        }
        
        public async Task<List<BloodTransferCenter>> GetByWilayaIdAsync(int wilayaId)
        {
            return await _context.BloodTransferCenters
                .Include(btc => btc.Wilaya)
                .Where(btc => btc.WilayaId == wilayaId)
                .ToListAsync();
        }
        
        public async Task<List<BloodTransferCenter>> GetAllAsync()
        {
            return await _context.BloodTransferCenters
                .Include(btc => btc.Wilaya)
                .ToListAsync();
        }
        
        public async Task<(List<BloodTransferCenter> Centers, int Total)> GetAllAsync(int page, int pageSize)
        {
            var query = _context.BloodTransferCenters
                .Include(btc => btc.Wilaya)
                .AsQueryable();
                
            var total = await query.CountAsync();
            
            var centers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
                
            return (centers, total);
        }
        
        // Method no longer needed with singleton approach
        public async Task SetAsPrimaryAsync(Guid id)
        {
            // No-op since we only have one center
            await Task.CompletedTask;
        }
        
        public async Task<BloodTransferCenter?> GetAsync()
        {
            return await _context.BloodTransferCenters
                .Include(btc => btc.Wilaya)
                .FirstOrDefaultAsync();
        }
        
        public async Task AddAsync(BloodTransferCenter center)
        {
            // Create an execution strategy that will handle retries
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                // Start transaction inside the execution strategy
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Check if any center already exists
                    bool hasCenter = await _context.BloodTransferCenters
                        .TagWith("Check_Single_Center")
                        .AnyAsync();
                    
                    if (hasCenter)
                    {
                        throw new InvalidOperationException("Only one Blood Transfer Center can exist in the system");
                    }
                    
                    await _context.BloodTransferCenters.AddAsync(center);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (PostgresException pgEx) when (pgEx.SqlState == "23505") // Unique violation
                {
                    // Handle unique constraint violation specifically
                    throw new InvalidOperationException("Cannot add another Blood Transfer Center due to unique constraint violation", pgEx);
                }
                catch (Exception ex)
                {
                    // Any other exception
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
        
        public async Task UpdateAsync(BloodTransferCenter center)
        {
            _context.BloodTransferCenters.Update(center);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(Guid id)
        {
            var center = await GetByIdAsync(id);
            if (center != null)
            {
                _context.BloodTransferCenters.Remove(center);
                await _context.SaveChangesAsync();
            }
        }
    }
}
