using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using cSharpTest.Models;

namespace cSharpTest.Migrations
{
    [DbContext(typeof(cSharpTestContext))]
    [Migration("20171129000431_OwnerNameMigration")]
    partial class OwnerNameMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("cSharpTest.Models.Activity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Desc");

                    b.Property<int>("Duration");

                    b.Property<string>("DurationScope");

                    b.Property<int>("OwnerId");

                    b.Property<string>("OwnerName");

                    b.Property<DateTime>("Time");

                    b.Property<string>("Title");

                    b.HasKey("id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("cSharpTest.Models.Participant", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivityId");

                    b.Property<int>("UserId");

                    b.HasKey("id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("UserId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("cSharpTest.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("cSharpTest.Models.Participant", b =>
                {
                    b.HasOne("cSharpTest.Models.Activity", "Activity")
                        .WithMany("Participants")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("cSharpTest.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
