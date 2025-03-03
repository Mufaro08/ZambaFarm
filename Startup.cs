using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Models;
using Microsoft.AspNetCore.Identity;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    /* public void ConfigureServices(IServiceCollection services)
     {
         services.AddDbContext<FarmContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("FarmContext")));

         services.AddDefaultIdentity<IdentityUser>()
             .AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<FarmContext>();

         services.AddControllersWithViews();
         services.AddRazorPages();
     }*/
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<FarmContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ZambaFarmContext")));

        services.AddControllersWithViews();
        // Add other services as needed
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
}
