using Final_Project.Models;

namespace Final_Project.ApiServices
{
    public interface ISpartaService<T>
    {

        Task<bool> CreateAsync(T entity, Spartan? user = null, string role = "Trainee");
        Task<bool> UpdateAsync(int id, T entity, Spartan? user = null, string role = "Trainee");
        Task<bool> DeleteAsync(int id, Spartan? user = null, string role = "Trainee");
        Task<IEnumerable<T>?> GetAllAsync(Spartan? user = null, string role = "Trainee");
        Task<T?> GetAsync(int id, Spartan? user = null, string role = "Trainee");
        Task SaveAsync();
    }
}
