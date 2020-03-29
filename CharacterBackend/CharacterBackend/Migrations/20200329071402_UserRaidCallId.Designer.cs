﻿// <auto-generated />
using System;
using CharacterBackend.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CharacterBackend.Migrations
{
    [DbContext(typeof(TeleQuestContext))]
    [Migration("20200329071402_UserRaidCallId")]
    partial class UserRaidCallId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CharacterBackend.DBContext.Models.CallLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<string>("UUID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CallLog");
                });

            modelBuilder.Entity("CharacterBackend.DBContext.Models.Quest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long>("ExpEarned")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Quests");
                });

            modelBuilder.Entity("CharacterBackend.DBContext.Models.Raid", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<long>("XpLevel")
                        .HasColumnType("bigint");

                    b.Property<long>("XpPenalty")
                        .HasColumnType("bigint");

                    b.Property<long>("XpReward")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Raid");
                });

            modelBuilder.Entity("CharacterBackend.DBContext.Models.RaidRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CallUUID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("RaidRooms");
                });

            modelBuilder.Entity("CharacterBackend.DBContext.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("ExpPoints")
                        .HasColumnType("bigint");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SessionKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PhoneNumber")
                        .IsUnique()
                        .HasFilter("[PhoneNumber] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CharacterBackend.DBContext.Models.UserRaid", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CallId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RaidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("damage")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RaidId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRaids");
                });

            modelBuilder.Entity("CharacterBackend.DBContext.Models.Quest", b =>
                {
                    b.HasOne("CharacterBackend.DBContext.Models.User", "User")
                        .WithMany("Quests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CharacterBackend.DBContext.Models.UserRaid", b =>
                {
                    b.HasOne("CharacterBackend.DBContext.Models.Raid", "Raid")
                        .WithMany("UserRaids")
                        .HasForeignKey("RaidId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CharacterBackend.DBContext.Models.User", "User")
                        .WithMany("UserRaids")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
