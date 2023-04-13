using Final_Project.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Final_Project.Data.ApiRepositories;

public class SpartaApiRepository<T> : ISpartaApiRepository<T> where T : class
{
    private readonly SpartaDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public SpartaApiRepository(SpartaDbContext context)
    {
        _dbSet = context.Set<T>();
        _context = context;
    }

    public bool IsNull => _dbSet == null;

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public virtual async Task<T?> FindAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
