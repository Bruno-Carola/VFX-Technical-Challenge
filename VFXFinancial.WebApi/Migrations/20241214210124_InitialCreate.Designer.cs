﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VFXFinancial.WebApi.Data;

#nullable disable

namespace VFXFinancial.WebApi.Migrations
{
    [DbContext(typeof(VFXFinancialDbContext))]
    [Migration("20241214210124_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VFXFinancial.WebApi.Models.Domain.ExchangeRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Ask")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<decimal>("Bid")
                        .HasPrecision(18, 8)
                        .HasColumnType("decimal(18,8)");

                    b.Property<string>("FromCurrency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("ToCurrency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("ExchangeRates");
                });
#pragma warning restore 612, 618
        }
    }
}
