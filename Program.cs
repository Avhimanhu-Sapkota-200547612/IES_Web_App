using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IES_WebAuth_Project.Areas.Identity.Data;
using IES_WebAuth_Project.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IES_WebAuth_Project
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Creates a web application builder.
            var builder = WebApplication.CreateBuilder(args);

            // Gets the database connection string from the configuration. 
            var connectionString = builder.Configuration.GetConnectionString("AppDBContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDBContextConnection' not found.");

            // Adds the application's database context to the dependency injection container, using the specified connection string and SQL Server as the database provider.
            builder.Services.AddDbContext<ApplicationDatabaseContext>(options => options.UseSqlServer(connectionString));

            // Adds the default Identity system to the services, which provides user and role management functionalities.
            builder.Services.AddDefaultIdentity<WebUser>(options => options.SignIn.RequireConfirmedAccount = true)
                 .AddRoles<IdentityRole>() // Enables role management for users.
                .AddEntityFrameworkStores<ApplicationDatabaseContext>(); // Uses the application's database context for storing Identity data.

            // Adds services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // Builds the application.
            var app = builder.Build();

            // Configures the HTTP request pipeline.

            // If the application is not in development mode, handles exceptions using the "UseExceptionHandler" middleware and enforces HTTPS using "UseHsts" middleware.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Redirects HTTP requests to HTTPS.
            app.UseHttpsRedirection();

            // Serves static files from the "wwwroot" folder.
            app.UseStaticFiles();

            // Enables routing.
            app.UseRouting();

            // Adds authorization middleware, which will check if the user is allowed to access certain resources.
            app.UseAuthorization();

            // Defines the default route for controllers.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Defines the default route for Razor Pages.
            app.MapRazorPages();

            // Uses a scope to manage the lifetime of services created during this block of code.
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Defines the roles here. This code checks if the specified roles exist in the database and creates them if they don't.
                string[] roles = { "Admin", "Manager", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            // Uses another scope to manage the lifetime of services created during this block of code.
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WebUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Defines the roles the users should be assigned to, along with the corresponding users' email addresses.
                var usersWithRoles = new Dictionary<string, string>
                {
                    { "admin@webapp.com", "Admin" },     // Assigns Administrator's role to the user
                    { "manager@webapp.com", "Manager" },      // Assigns Manager's role to the user
                    { "user@webapp.com", "User" }         // Assigns General User's role to the user
                };

                // For each user-role pair, checks if the user exists. If not, create the user and assign the specified role.
                foreach (var userRole in usersWithRoles)
                {
                    var email = userRole.Key;
                    var role = userRole.Value;

                    if (await userManager.FindByEmailAsync(email) == null)
                    {
                        // Creates a new user with the specified email and set EmailConfirmed to true.
                        var user = new WebUser { UserName = email, Email = email, EmailConfirmed = true };
                        // Set the default password for all the user.
                        await userManager.CreateAsync(user, "Pass#123"); 

                        // Assigns the role to the user if the role exists in the database.
                        if (await roleManager.RoleExistsAsync(role))
                        {
                            await userManager.AddToRoleAsync(user, role);
                        }
                    }
                }
            }

            // Runs the application.
            app.Run();
        }
    }
}
