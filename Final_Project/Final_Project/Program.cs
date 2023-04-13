using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Final_Project.Data.Repositories;
using Final_Project.Data.ApiRepositories;
using Final_Project.ApiServices;

namespace Final_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<SpartaDbContext>(options =>
                options.UseSqlServer(connectionString));


            builder.Services.AddScoped(
                 typeof(ISpartaApiRepository<>),
                 typeof(SpartaApiRepository<>));

            builder.Services.AddScoped(
                 typeof(ISpartaApiService<>),
                 typeof(SpartaApiService<>));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<Spartan>
                (options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SpartaDbContext>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                SeedData.Initialise(scope.ServiceProvider);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}