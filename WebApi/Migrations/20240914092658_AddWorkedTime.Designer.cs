﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.DbContexts;

#nullable disable

namespace WebApi.Migrations
{
    [DbContext(typeof(PlatesDbContext))]
    [Migration("20240914092658_AddWorkedTime")]
    partial class AddWorkedTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("WebApi.Entities.Plate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsLoading")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LicenseText")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("WorkedTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Plates");
                });
#pragma warning restore 612, 618
        }
    }
}
