﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository;

#nullable disable

namespace MntPlus.API.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20240403234856_DatabaseCreation")]
    partial class DatabaseCreation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Equipment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("EquipmentId");

                    b.Property<string>("EquipmentCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("EquipmentImage")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("EquipmentMake")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EquipmentModel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EquipmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EquipmentNameImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("EquipmentParent")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Equipment");
                });
#pragma warning restore 612, 618
        }
    }
}
