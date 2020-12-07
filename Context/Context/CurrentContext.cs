using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Context.Entities;
using System.Linq;

namespace Context
{
    public class CurrentContext : IdentityDbContext<User, Role, int>
    {   
        public CurrentContext(DbContextOptions<CurrentContext> options, IConfiguration configuration) : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
          .ToTable("Users", "User");

            modelBuilder.Entity<Role>()
                .ToTable("Roles", "User");

        }
    }
}
