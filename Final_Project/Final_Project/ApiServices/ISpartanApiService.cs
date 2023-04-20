namespace Final_Project.ApiServices
{
    public interface ISpartanApiService<T>
    {
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(string id, T entity);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T?> GetAsync(string id);
        Task SaveAsync();

    }
}
