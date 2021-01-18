﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QARS.Data;

namespace QARS.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210117215730_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9");

            modelBuilder.Entity("QARS.Data.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Available")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(16);

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Mileage")
                        .HasColumnType("TEXT");

                    b.Property<int>("ModelId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Requested")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StoreId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("ModelId");

                    b.HasIndex("StoreId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("QARS.Data.Models.CarModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<int>("Category")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("DayRate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasMaxLength(1000);

                    b.Property<int>("Doors")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Efficiency")
                        .HasColumnType("REAL");

                    b.Property<int>("Emission")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("FreeMileage")
                        .HasColumnType("TEXT");

                    b.Property<int>("FuelType")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasAirconditioning")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Image")
                        .HasColumnType("BLOB");

                    b.Property<decimal>("KMRate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Passengers")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SuitCases")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Transmission")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("CarModels");
                });

            modelBuilder.Entity("QARS.Data.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Contactaddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Countrycode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discript")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Image")
                        .HasColumnType("BLOB");

                    b.Property<string>("Tell")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("QARS.Data.Models.Extra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("StoreId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Extras");
                });

            modelBuilder.Entity("QARS.Data.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("QARS.Data.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CarId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CarLocationId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("End")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InitialMileage")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Start")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int>("dropoffLocation")
                        .HasColumnType("INTEGER");

                    b.Property<int>("pickupLocation")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("CarLocationId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("QARS.Data.Models.ReservationExtra", b =>
                {
                    b.Property<int?>("ReservationId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ExtraId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReservationId", "ExtraId");

                    b.HasIndex("ExtraId");

                    b.ToTable("ReservationExtras");
                });

            modelBuilder.Entity("QARS.Data.Models.Return", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ReservationsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ReservationsId");

                    b.ToTable("Returns");
                });

            modelBuilder.Entity("QARS.Data.Models.Role", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("QARS.Data.Models.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT")
                        .HasMaxLength(2500);

                    b.Property<int?>("FranchiseeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FranchiseeId");

                    b.HasIndex("LocationId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("QARS.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Password")
                        .HasColumnType("BLOB")
                        .HasMaxLength(64);

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("LocationId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("QARS.Data.Models.UserRole", b =>
                {
                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("QARS.Data.Models.Customer", b =>
                {
                    b.HasBaseType("QARS.Data.Models.User");

                    b.Property<byte[]>("DriversLicenseBack")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("DriversLicenseFront")
                        .HasColumnType("BLOB");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("QARS.Data.Models.Employee", b =>
                {
                    b.HasBaseType("QARS.Data.Models.User");

                    b.Property<int>("FranchiseeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StoreId")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Employee");
                });

            modelBuilder.Entity("QARS.Data.Models.Franchisee", b =>
                {
                    b.HasBaseType("QARS.Data.Models.User");

                    b.HasDiscriminator().HasValue("Franchisee");
                });

            modelBuilder.Entity("QARS.Data.Models.Administrator", b =>
                {
                    b.HasBaseType("QARS.Data.Models.Employee");

                    b.HasDiscriminator().HasValue("Administrator");
                });

            modelBuilder.Entity("QARS.Data.Models.Car", b =>
                {
                    b.HasOne("QARS.Data.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QARS.Data.Models.CarModel", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QARS.Data.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QARS.Data.Models.Reservation", b =>
                {
                    b.HasOne("QARS.Data.Models.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QARS.Data.Models.Location", "CarLocation")
                        .WithMany()
                        .HasForeignKey("CarLocationId");

                    b.HasOne("QARS.Data.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");
                });

            modelBuilder.Entity("QARS.Data.Models.ReservationExtra", b =>
                {
                    b.HasOne("QARS.Data.Models.Extra", "Extra")
                        .WithMany("ReservationExtras")
                        .HasForeignKey("ExtraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QARS.Data.Models.Reservation", "Reservation")
                        .WithMany("Extras")
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QARS.Data.Models.Return", b =>
                {
                    b.HasOne("QARS.Data.Models.Reservation", "Reservations")
                        .WithMany()
                        .HasForeignKey("ReservationsId");
                });

            modelBuilder.Entity("QARS.Data.Models.Store", b =>
                {
                    b.HasOne("QARS.Data.Models.Franchisee", "Franchisee")
                        .WithMany()
                        .HasForeignKey("FranchiseeId");

                    b.HasOne("QARS.Data.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QARS.Data.Models.User", b =>
                {
                    b.HasOne("QARS.Data.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("QARS.Data.Models.UserRole", b =>
                {
                    b.HasOne("QARS.Data.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QARS.Data.Models.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
