﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Railway_Management.Models;

#nullable disable

namespace Railway_Management.Migrations
{
    [DbContext(typeof(ConnectionContext))]
    [Migration("20241222191931_customerpersonaldetails")]
    partial class customerpersonaldetails
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+AllStations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("latitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("longitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("state")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("station_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("station_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("zone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AllStations1");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+Booking", b =>
                {
                    b.Property<int>("BookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingID"));

                    b.Property<string>("ArrivalTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BStationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BStationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookingDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<string>("DepartureTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Distance")
                        .HasColumnType("float");

                    b.Property<int>("Fare")
                        .HasColumnType("int");

                    b.Property<string>("StationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrainName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrainNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookingID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"));

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+CustomerPersonalDetails", b =>
                {
                    b.Property<int>("detailsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("detailsID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pincode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("detailsID");

                    b.HasIndex("CustomerID");

                    b.ToTable("CustomerPersonalDetails");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+Fare", b =>
                {
                    b.Property<int>("FareID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FareID"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrainID")
                        .HasColumnType("int");

                    b.HasKey("FareID");

                    b.ToTable("Fares");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+Station", b =>
                {
                    b.Property<int>("StationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StationID"));

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StationID");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+Train", b =>
                {
                    b.Property<int>("TrainID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrainID"));

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrainName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TrainID");

                    b.ToTable("Trains");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+TrainDetails", b =>
                {
                    b.Property<int>("trainID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("trainID"));

                    b.Property<string>("Classes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan?>("arrival_time")
                        .HasColumnType("time");

                    b.Property<int?>("chair_car")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("departure_time")
                        .HasColumnType("time");

                    b.Property<int>("distance")
                        .HasColumnType("int");

                    b.Property<int?>("distance_cover")
                        .HasColumnType("int");

                    b.Property<int?>("duration_hrs")
                        .HasColumnType("int");

                    b.Property<int?>("duration_min")
                        .HasColumnType("int");

                    b.Property<int?>("first_class")
                        .HasColumnType("int");

                    b.Property<int>("firstac")
                        .HasColumnType("int");

                    b.Property<string>("from_station_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("from_station_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("returntrain")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("seconndac")
                        .HasColumnType("int");

                    b.Property<int?>("sleeper")
                        .HasColumnType("int");

                    b.Property<int?>("third_ac")
                        .HasColumnType("int");

                    b.Property<string>("tostationcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tostationname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("trainNO")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("trainName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("trainType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("zone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("trainID");

                    b.ToTable("Train_Details");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+TrainSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Arrival")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Departure")
                        .HasColumnType("time");

                    b.Property<string>("Station_Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Station_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Train_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Train_Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TrainSchedule");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+TrainsRunningCoordinates", b =>
                {
                    b.Property<int>("Coordinatesid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Coordinatesid"));

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("trainID")
                        .HasColumnType("int");

                    b.HasKey("Coordinatesid");

                    b.HasIndex("trainID");

                    b.ToTable("Trains_Running_Coordinates");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+Booking", b =>
                {
                    b.HasOne("Railway_Management.Models.AllDataDetails+Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+CustomerPersonalDetails", b =>
                {
                    b.HasOne("Railway_Management.Models.AllDataDetails+Customer", "customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");
                });

            modelBuilder.Entity("Railway_Management.Models.AllDataDetails+TrainsRunningCoordinates", b =>
                {
                    b.HasOne("Railway_Management.Models.AllDataDetails+TrainDetails", "TrainDetails")
                        .WithMany()
                        .HasForeignKey("trainID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
