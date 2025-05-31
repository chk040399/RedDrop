using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class BloodTransferCenterRepository : IBloodTransferCenterRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BloodTransferCenterRepository> _logger;
        
        public BloodTransferCenterRepository(
            ApplicationDbContext context,
            ILogger<BloodTransferCenterRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<BloodTransferCenter?> GetAsync()
        {
            return await _context.BloodTransferCenters
                .Include(btc => btc.Wilaya)
                .FirstOrDefaultAsync();
        }
        
        public async Task<bool> ExistsAsync()
        {
            return await _context.BloodTransferCenters.AnyAsync();
        }
        
        public async Task SaveAsync(BloodTransferCenter center)
        {
            // Check if a center already exists
            if (await ExistsAsync())
            {
                _logger.LogWarning("Attempted to create a second blood transfer center. Only one center is allowed.");
                throw new InvalidOperationException("Only one blood transfer center is allowed in the system.");
            }
            
            await _context.BloodTransferCenters.AddAsync(center);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Blood transfer center created with ID: {Id}", center.Id);
        }
        
        public async Task UpdateAsync(BloodTransferCenter center)
        {
            _context.BloodTransferCenters.Update(center);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Blood transfer center updated: {Id}", center.Id);
        }
        
        public async Task DeleteAsync()
        {
            var center = await GetAsync();
            if (center != null)
            {
                _context.BloodTransferCenters.Remove(center);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Blood transfer center deleted");
            }
        }
    }
}
