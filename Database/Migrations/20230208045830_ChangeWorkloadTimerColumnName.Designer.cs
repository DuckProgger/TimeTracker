﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230208045830_ChangeWorkloadTimerColumnName")]
    partial class ChangeWorkloadTimerColumnName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Entity.Work", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkdayId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("Workload")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("WorkdayId");

                    b.ToTable("Works");
                });

            modelBuilder.Entity("Entity.Workday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Workdays");
                });

            modelBuilder.Entity("Entity.WorkloadTimer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartRecordingDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorkId")
                        .IsUnique();

                    b.ToTable("WorkloadTimers");
                });

            modelBuilder.Entity("Entity.Work", b =>
                {
                    b.HasOne("Entity.Workday", "Workday")
                        .WithMany("Works")
                        .HasForeignKey("WorkdayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workday");
                });

            modelBuilder.Entity("Entity.WorkloadTimer", b =>
                {
                    b.HasOne("Entity.Work", "Work")
                        .WithOne("WorkloadTimer")
                        .HasForeignKey("Entity.WorkloadTimer", "WorkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Work");
                });

            modelBuilder.Entity("Entity.Work", b =>
                {
                    b.Navigation("WorkloadTimer");
                });

            modelBuilder.Entity("Entity.Workday", b =>
                {
                    b.Navigation("Works");
                });
#pragma warning restore 612, 618
        }
    }
}