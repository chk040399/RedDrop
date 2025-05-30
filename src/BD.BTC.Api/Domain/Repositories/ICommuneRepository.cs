using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICommuneRepository
    {
        Task<Commune?> GetByIdAsync(int id);
        Task<Commune?> GetByNameAsync(string name);
        Task<List<Commune>> GetByWilayaIdAsync(int wilayaId);
        Task<List<Commune>> GetAllAsync();
        Task<(List<Commune> Communes, int Total)> GetAllAsync(int page, int pageSize, string? name = null, int? wilayaId = null);
        Task AddAsync(Commune commune);
        Task UpdateAsync(Commune commune);
        Task DeleteAsync(int id);
    }
}
