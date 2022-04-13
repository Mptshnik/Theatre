using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theatre.Models;

namespace Theatre.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {          
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().Property(e => e.Login).HasDefaultValue("_");
            modelBuilder.Entity<Employee>().Property(e => e.Password).HasDefaultValue("_");
            modelBuilder.Entity<Employee>().Property(e => e.Rewards).HasDefaultValue("_");
            modelBuilder.Entity<Employee>().Property(e => e.MiddleName).HasDefaultValue("");

            modelBuilder.Entity<Employee>().HasOne(p => p.Post).WithMany(t => t.Employees).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Ticket>().HasOne(p => p.Performance).WithMany(t => t.Tickets).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Performance>().HasOne(p => p.Author).WithMany(t => t.Performances).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Membership>().HasOne(p => p.Author).WithMany(t => t.Memberships).OnDelete(DeleteBehavior.SetNull);           
        }

      
    }
}
