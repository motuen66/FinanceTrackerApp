using FinanceTracker.Services.Interfaces.Repositories;
using FinanceTracker.Services.Interfaces.Services;

namespace FinanceTracker.Services
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _genericRepository;
        public GenericService(IGenericRepository<TEntity> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _genericRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _genericRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _genericRepository.GetAllAsync();
        }

        public Task<TEntity> GetByIdAsync(string id)
        {
            return _genericRepository.GetByIdAsync(id);
        }

        public Task UpdateAsync(TEntity entity)
        {
            return _genericRepository.UpdateAsync(entity);
        }
    }
}
