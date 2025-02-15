﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using _301106599_mahmud_final_project.Data;

#nullable disable

namespace _301106599_mahmud_final_project.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("_301106599_mahmud_final_project.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Category");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            Name = "Electronics"
                        },
                        new
                        {
                            CategoryId = 2,
                            Name = "Clothing"
                        },
                        new
                        {
                            CategoryId = 3,
                            Name = "Books"
                        },
                        new
                        {
                            CategoryId = 4,
                            Name = "Furniture"
                        },
                        new
                        {
                            CategoryId = 5,
                            Name = "Appliances"
                        });
                });

            modelBuilder.Entity("_301106599_mahmud_final_project.Models.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocationId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocationId");

                    b.ToTable("Location");

                    b.HasData(
                        new
                        {
                            LocationId = 1,
                            Name = "Toronto"
                        },
                        new
                        {
                            LocationId = 2,
                            Name = "Mississauga"
                        },
                        new
                        {
                            LocationId = 3,
                            Name = "Brampton"
                        },
                        new
                        {
                            LocationId = 4,
                            Name = "Scarborough"
                        });
                });

            modelBuilder.Entity("_301106599_mahmud_final_project.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SerialNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId");

                    b.ToTable("Product");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryId = 1,
                            Date = new DateOnly(2021, 1, 1),
                            Description = "Dell Laptop",
                            ImageUrl = "https://www.dell.com/en-ca/shop/laptops-ultrabooks/xps-15-laptop/spd/xps-15-9530-laptop/caexcpbts9530gvhd",
                            LocationId = 1,
                            Name = "Dell XPS",
                            Price = 1000m,
                            Quantity = 10,
                            SerialNo = "123456"
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryId = 2,
                            Date = new DateOnly(2021, 1, 1),
                            Description = "Nike T-Shirt",
                            ImageUrl = "https://www.jiffy.com/ca/mo-4800J1.html?ac=Sapphire&utm_adgroup=163642421436&utm_term=&campcat=&target=pla-2313723458585&physical=9001383&matchtype=&adtype=pla&jid=JS999985&pgid=2313723458585&net=g&m=&search=[true]&creative=702815230789&where=&country=CA&bookmark=8585128489953639874&gad_source=1",
                            LocationId = 2,
                            Name = "T-Shirt",
                            Price = 20m,
                            Quantity = 100,
                            SerialNo = "123457"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
