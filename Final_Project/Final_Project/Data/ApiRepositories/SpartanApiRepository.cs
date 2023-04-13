using Final_Project.Data;
using Final_Project.Data.ApiRepositories;
using Final_Project.Models;
using Microsoft.EntityFrameworkCore;



namespace NorthwindAPI_MiniProject.Data.Repository
{
    public class SpartanApiRepository : SpartaApiRepository<Spartan>, ISpartanApiRepository<Spartan>
    {
        public SpartanApiRepository(SpartaDbContext context) : base(context)
        {

        }

        public async Task<Spartan?> FindAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

    }
}