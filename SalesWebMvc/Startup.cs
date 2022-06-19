using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
using SalesWebMvc.Services;
namespace SalesWebMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllersWithViews();

            var serverVersion = new MySqlServerVersion(new Version(8, 0));

            services.AddDbContext<SalesWebMvcContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), serverVersion)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                );

            //services.AddScoped<SeedingService>();
            services.AddScoped<SellerService>();
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();

            }

            //SeedingService.Seed();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //seed database 
            AppDbInitializer.Seed(app);

        }
    }

}
