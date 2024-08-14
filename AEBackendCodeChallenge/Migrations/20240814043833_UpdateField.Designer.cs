﻿// <auto-generated />
using System;
using AEBackendCodeChallenge.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AEBackendCodeChallenge.Migrations
{
    [DbContext(typeof(ShipDbContext))]
    [Migration("20240814043833_UpdateField")]
    partial class UpdateField
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AEBackendCodeChallenge.Models.Port", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Ports");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Latitude = 34.052199999999999,
                            Longitude = -118.2437,
                            Name = "Los Angeles Port"
                        },
                        new
                        {
                            Id = 2,
                            Latitude = 40.712800000000001,
                            Longitude = -74.006,
                            Name = "New York Port"
                        },
                        new
                        {
                            Id = 3,
                            Latitude = 51.507399999999997,
                            Longitude = -0.1278,
                            Name = "London Port"
                        });
                });

            modelBuilder.Entity("AEBackendCodeChallenge.Models.Ship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShipId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<double>("Velocity")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Ships");
                });

            modelBuilder.Entity("AEBackendCodeChallenge.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AEBackendCodeChallenge.Models.Ship", b =>
                {
                    b.HasOne("AEBackendCodeChallenge.Models.User", "User")
                        .WithMany("Ships")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AEBackendCodeChallenge.Models.User", b =>
                {
                    b.Navigation("Ships");
                });
#pragma warning restore 612, 618
        }
    }
}
