using Final_Project.Data;
using Final_Project.Data.ApiRepositories;
using Final_Project.Data.Repositories;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Final_Project.ApiServices;

public class SpartanApiService<T> : ISpartanApiService<T> where T : class
{
    protected readonly ISpartanApiRepository<T> _repository;

    public SpartanApiService(ISpartanApiRepository<T> respository)
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

    public async Task<bool> DeleteAsync(string id)
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

    public async Task<T?> GetAsync(string id)
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

    public async Task<bool> UpdateAsync(string id, T entity)
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
