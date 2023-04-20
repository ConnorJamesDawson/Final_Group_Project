using Final_Project.Data.Repositories;
using Final_Project.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.ApiServices;

public class SpartaApiService<T> : ISpartaApiService<T> where T : class
{
    protected readonly ISpartaApiRepository<T> _repository;

    public SpartaApiService(ISpartaApiRepository<T> respository)
    {
        _repository = respository;
    }

    public async Task<bool> CreateAsync(T entity)
    {
        if (_repository.IsNull || entity == null)
        {
            return false;
        }
        else
        {
            _repository.Add(entity);
            await _repository.SaveAsync();
            return true;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (_repository.IsNull)
        {
            return false;
        }

        var entity = await _repository.FindAsync(id);

        if (entity == null)
        {
            return false;
        }

        _repository.Remove(entity);

        await _repository.SaveAsync();

        return true;
    }

    public async Task<IEnumerable<T>?> GetAllAsync()
    {

        if (_repository.IsNull)
        {
            return null;
        }
        return (await _repository.GetAllAsync())
            .ToList();
    }

    public async Task<T?> GetAsync(int id)
    {
        if (_repository.IsNull)
        {
            return null;
        }

        T entity = await _repository.FindAsync(id);

        if (entity == null)
        {

            return null;
        }

        return entity;
    }

    public async Task SaveAsync()
    {
        _repository.SaveAsync();
    }

    public async Task<bool> UpdateAsync(int id, T entity)
    {
        _repository.Update(entity);
        try
        {
            await _repository.SaveAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
        return true;
    }
}
