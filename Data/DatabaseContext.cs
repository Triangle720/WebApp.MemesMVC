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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserModel>()
                .HasData(
                new UserModel()
                {
                    Login = "admin",
                    Nickname = "DefinitlyNotAnAdmin",
                    Password = "62F04A011FBB80030BB0A13701C20B41",
                    Email = "admin@admin.net",
                    AccountCreationTime = DateTime.Now,
                    Role = RoleTypes.ADMIN
                },
                new UserModel()
                {
                    Login = "moderator",
                    Nickname = "moderator",
                    Password = "0408F3C997F309C03B08BF3A4BC7B730",
                    Email = "mod@mod.net",
                    AccountCreationTime = DateTime.Now,
                    Role = RoleTypes.ADMIN
                 });
        }
    }
}

