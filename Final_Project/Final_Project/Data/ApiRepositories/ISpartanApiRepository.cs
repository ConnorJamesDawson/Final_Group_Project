using Final_Project.Data.Repositories;

namespace Final_Project.Data.ApiRepositories
{
    public interface ISpartanApiRepository<T> : ISpartaApiRepository<T> where T : class
    {
        Task<T?> FindAsync(string id);
    }
}
