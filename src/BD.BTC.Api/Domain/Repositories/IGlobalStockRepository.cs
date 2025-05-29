using Domain.Entities;
using Domain.ValueObjects;
namespace Domain.Repositories
{
    public interface IGlobalStockRepository
    {
        Task<GlobalStock?> GetByKeyAsync(BloodType bloodGroup, BloodBagType bloodBagType);
        Task<GlobalStock?> GetByBloodBagTypeAsync(BloodBagType bloodBagType);
        Task<GlobalStock?> GetByBloodGroupAsync(BloodType bloodGroup);
        Task<List<GlobalStock>> GetAllAsync(BloodType? bloodType = null, BloodBagType? bloodBagType = null);
        Task AddAsync(GlobalStock globalStock);
        Task UpdateAsync(GlobalStock globalStock);
        Task DeleteAsync(BloodType BT, BloodBagType BBT);
    }
}