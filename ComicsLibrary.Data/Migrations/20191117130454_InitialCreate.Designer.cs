﻿// <auto-generated />
using System;
using ComicsLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ComicsLibrary.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191117130454_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("ComicsLibrary")
                .HasAnnotation("ProductVersion", "3.1.0-preview3.19554.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ComicsLibrary.Common.Models.Comic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Creators")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("DiamondCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Isbn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("IssueNumber")
                        .HasColumnType("float");

                    b.Property<int?>("MarvelId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("OnSaleDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ReadUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReadUrlAdded")
                        .HasColumnType("datetime2");

                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<bool>("ToReadNext")
                        .HasColumnType("bit");

                    b.Property<string>("Upc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SeriesId");

                    b.ToTable("ComicSeries");
                });

            modelBuilder.Entity("ComicsLibrary.Common.Models.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Abandoned")
                        .HasColumnType("bit");

                    b.Property<string>("Characters")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EndYear")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MarvelId")
                        .HasColumnType("int");

                    b.Property<int?>("Order")
                        .HasColumnType("int");

                    b.Property<int?>("StartYear")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("ComicsLibrary.Common.Models.Comic", b =>
                {
                    b.HasOne("ComicsLibrary.Common.Models.Series", "Series")
                        .WithMany("Comics")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
