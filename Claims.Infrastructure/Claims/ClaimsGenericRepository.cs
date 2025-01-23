using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Claims;

public class ClaimsGenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ClaimsContext _context;
    private readonly DbSet<T> _dbSet;

    public ClaimsGenericRepository(ClaimsContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }
}
