using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ZambaFarm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // Configure services here
    public void ConfigureServices(IServiceCollection services)
    {
        // Configure Entity Framework with SQL Server
        services.AddDbContext<FarmContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FarmContext")));

        // Configure Identity with roles and EF Core store
        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<FarmContext>();

        // Register the IEmailSender implementation
        services.AddTransient<IEmailSender, EmailSender>();

        // Add controllers and views
        services.AddControllersWithViews();

        // Add Razor Pages
        services.AddRazorPages();
    }

    // Configure middleware here
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

        app.UseAuthentication(); // Enable Authentication
        app.UseAuthorization(); // Enable Authorization

        app.UseEndpoints(endpoints =>
        {
            // Set up default route for MVC
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Enable Razor Pages
            endpoints.MapRazorPages();
        });
    }
}