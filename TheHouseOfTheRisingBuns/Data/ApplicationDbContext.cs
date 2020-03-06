using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheHouseOfTheRisingBuns.Models;

namespace TheHouseOfTheRisingBuns.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<TheHouseOfTheRisingBuns.Models.Recipe> Recipe { get; set; }

        public DbSet<TheHouseOfTheRisingBuns.Models.Category> Category { get; set; }

        public DbSet<TheHouseOfTheRisingBuns.Models.SignUp> SignUp { get; set; }

       
    }
}
