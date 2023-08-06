
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IES_WebAuth_Project.Areas.Identity.Data;
using IES_WebAuth_Project.Models;

// Namespace for the application's data context
namespace IES_WebAuth_Project.Data
{
    // Class representing the application's database context, which is derived from IdentityDbContext
    public class ApplicationDatabaseContext : IdentityDbContext<WebUser>
    {
        // Constructor taking DbContextOptions as input, used to configure the database context
        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options)
            : base(options)
        {
        }

        // DbSet representing the "Contacts" table in the database, used to interact with Contact entities
        public DbSet<Contact> Contacts { get; set; }

        // Method for configuring the model during the database context creation
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call the base class's method to apply default model configurations for identity tables
            base.OnModelCreating(builder);
        }
    }
}
