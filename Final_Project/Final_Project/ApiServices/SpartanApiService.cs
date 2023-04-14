using Final_Project.Data.ApiRepositories;
using Final_Project.Data.Repositories;
using Final_Project.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Final_Project.ApiServices;

public class SpartanApiService : ISpartanApiService<Spartan>
{
    protected readonly ISpartanApiRepository<Spartan> _repository;

    public SpartanApiService(ISpartanApiRepository<Spartan> respository)
    {
        _repository = respository;
    }

    public async Task<bool> CreateAsync(Spartan entity)
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

    public async Task<IEnumerable<Spartan>?> GetAllAsync()
    {

        if (_repository.IsNull)
        {
            return null;
        }
        return (await _repository.GetAllAsync())
            .ToList();
    }

    public async Task<Spartan?> GetAsync(string id)
    {
        if (_repository.IsNull)
        {
            return null;
        }

        Spartan entity = await _repository.FindAsync(id);

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

    public async Task<bool> UpdateAsync(string id, Spartan entity)
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
