using Final_Project.ApiServices;
using Final_Project.Data;
using Final_Project.Data.ApiRepositories;
using Final_Project.Data.Repositories;
using Final_Project.Models;
using Final_Project.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Final_Project.Data.Repositories;
using Final_Project.Data.ApiRepositories;
using Final_Project.ApiServices;
using NorthwindAPI_MiniProject.Data.Repository;
using Final_Project.MVCService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            builder.Services.AddAuthentication().AddJwtBearer();

            builder.Services.AddScoped(
                 typeof(ISpartaApiRepository<>),
                 typeof(SpartaApiRepository<>));

            builder.Services.AddScoped(
                 typeof(ISpartaApiService<>),
                 typeof(SpartaApiService<>));
           

            builder.Services.AddScoped(
                typeof(ISpartanApiRepository<>),
                typeof(SpartanApiRepository<>));

            builder.Services.AddScoped(
                typeof(ISpartanApiService<>),
                typeof(SpartanApiService<>));


            builder.Services.AddScoped(
            typeof(TraineeProfilesService),
            typeof(TraineeProfilesService));

            builder.Services.AddScoped(
                typeof(PersonalTrackerService),
                typeof(PersonalTrackerService));

            builder.Services.AddScoped<ISpartaApiRepository<TraineeProfile>,
                SpartaApiRepository<TraineeProfile>>();

            builder.Services.AddScoped<ISpartaApiService<TraineeProfile>,
                SpartaApiService<TraineeProfile>>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<Spartan>
                (options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SpartaDbContext>()
                .AddSignInManager<SignInManager<Spartan>>();



            builder.Services.AddControllers()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            builder.Services.AddEndpointsApiExplorer();

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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}