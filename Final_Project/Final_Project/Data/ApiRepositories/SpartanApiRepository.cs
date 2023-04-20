using Final_Project.Data;
using Final_Project.Data.ApiRepositories;
using Final_Project.Models;
using Microsoft.EntityFrameworkCore;



namespace NorthwindAPI_MiniProject.Data.Repository
{
    public class SpartanApiRepository<T> : SpartaApiRepository<T>, ISpartanApiRepository<T> where T : class
    {
        public SpartanApiRepository(SpartaDbContext context) : base(context)
        {

        }

        public async Task<T?> FindAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

    }
}