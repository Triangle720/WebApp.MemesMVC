using Microsoft.EntityFrameworkCore;
using System;
using WebApp.MemesMVC.Models;

namespace WebApp.MemesMVC.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<PictureModel> Pictures { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<RoleModel>()
                .HasData(
                new RoleModel { Id = 1, RoleName = Enum.GetName(typeof(RoleTypes), RoleTypes.USER) },
                new RoleModel { Id = 2, RoleName = Enum.GetName(typeof(RoleTypes), RoleTypes.MODERATOR) },
                new RoleModel { Id = 3, RoleName = Enum.GetName(typeof(RoleTypes), RoleTypes.ADMIN) }
                );
        }
    }
}
