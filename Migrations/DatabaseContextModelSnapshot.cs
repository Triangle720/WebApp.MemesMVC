﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp.MemesMVC.Data;

namespace WebApp.MemesMVC.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApp.MemesMVC.Models.PictureModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<string>("LocalPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UrlAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("UserModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserModelId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("WebApp.MemesMVC.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AccountCreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("BanExpireIn")
                        .HasColumnType("datetime2");

                    b.Property<string>("BanReason")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<bool>("IsBanned")
                        .HasColumnType("bit");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccountCreationTime = new DateTime(2020, 10, 13, 2, 31, 5, 320, DateTimeKind.Local).AddTicks(2298),
                            BanReason = "",
                            Email = "admin@admin.net",
                            IsBanned = false,
                            Login = "admin",
                            Nickname = "DefinitlyNotAnAdmin",
                            Password = "62F04A011FBB80030BB0A13701C20B41",
                            Role = 2
                        },
                        new
                        {
                            Id = 2,
                            AccountCreationTime = new DateTime(2020, 10, 13, 2, 31, 5, 320, DateTimeKind.Local).AddTicks(3902),
                            BanReason = "",
                            Email = "mod@mod.net",
                            IsBanned = false,
                            Login = "moderator",
                            Nickname = "moderator",
                            Password = "0408F3C997F309C03B08BF3A4BC7B730",
                            Role = 2
                        });
                });

            modelBuilder.Entity("WebApp.MemesMVC.Models.PictureModel", b =>
                {
                    b.HasOne("WebApp.MemesMVC.Models.UserModel", null)
                        .WithMany("Pictures")
                        .HasForeignKey("UserModelId");
                });
#pragma warning restore 612, 618
        }
    }
}
