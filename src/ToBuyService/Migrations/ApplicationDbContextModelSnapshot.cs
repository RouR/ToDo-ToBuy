﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ToBuyService.DAL;

namespace ToBuyService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Domain.DBEntities.TobuyEntity", b =>
                {
                    b.Property<Guid>("PublicId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime?>("DueToUtc");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<float>("Qty");

                    b.Property<DateTime>("Updated");

                    b.Property<Guid>("UserId");

                    b.HasKey("PublicId");

                    b.HasIndex("UserId", "IsDeleted");

                    b.ToTable("Tobuy");
                });

            modelBuilder.Entity("Domain.DBEntities.TobuyEntity", b =>
                {
                    b.OwnsOne("Domain.Models.Price", "Price", b1 =>
                        {
                            b1.Property<Guid>("TobuyEntityPublicId");

                            b1.Property<decimal>("Amount");

                            b1.Property<int>("Currency");

                            b1.HasKey("TobuyEntityPublicId");

                            b1.ToTable("Tobuy");

                            b1.HasOne("Domain.DBEntities.TobuyEntity")
                                .WithOne("Price")
                                .HasForeignKey("Domain.Models.Price", "TobuyEntityPublicId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
