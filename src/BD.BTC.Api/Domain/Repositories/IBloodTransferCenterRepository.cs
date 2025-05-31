using Domain.Entities;

namespace Domain.Repositories
{
    public interface IBloodTransferCenterRepository
    {
        Task<BloodTransferCenter?> GetAsync();
        Task<bool> ExistsAsync();
        Task SaveAsync(BloodTransferCenter center);
        Task UpdateAsync(BloodTransferCenter center);
        Task DeleteAsync();
    }
}
