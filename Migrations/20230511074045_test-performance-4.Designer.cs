﻿// <auto-generated />
using System;
using AxeAssessmentToolWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230511074045_test-performance-4")]
    partial class testperformance4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.Option", b =>
                {
                    b.Property<int>("OptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OptionId"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("OptionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Options");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionId"));

                    b.Property<string>("QuestionContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionImage")
                        .HasColumnType("nvarchar(650)");

                    b.Property<int>("QuestionTypeId")
                        .HasColumnType("int");

                    b.HasKey("QuestionId");

                    b.HasIndex("QuestionTypeId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.QuestionType", b =>
                {
                    b.Property<int>("QuestionTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionTypeId"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("QuestionTypeId");

                    b.ToTable("QuestionType");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("College")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(60)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(60)");

                    b.Property<int>("SelectionStatus")
                        .HasColumnType("int");

                    b.Property<string>("UserProfileImage")
                        .HasColumnType("nvarchar(700)");

                    b.Property<string>("UserResumeFileName")
                        .HasColumnType("nvarchar(650)");

                    b.Property<string>("UserToken")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(60)");

                    b.Property<bool?>("isAdmin")
                        .HasColumnType("bit");

                    b.Property<int>("violation")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.UserTest", b =>
                {
                    b.Property<int>("UserTestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserTestId"));

                    b.Property<int>("Attempted")
                        .HasColumnType("int");

                    b.Property<bool>("EndTest")
                        .HasColumnType("bit");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserTestId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserTests");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.User_OptionSelected", b =>
                {
                    b.Property<int>("OptionSelctedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OptionSelctedId"));

                    b.Property<int>("OptionIndex")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("OptionSelctedId");

                    b.HasIndex("QuestionId");

                    b.ToTable("User_OptionSelected");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.User_QuestionAttempted", b =>
                {
                    b.Property<int>("QuestionAttemptedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionAttemptedId"));

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("QuestionAttemptedId");

                    b.HasIndex("UserId");

                    b.ToTable("User_QuestionAttempted");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.Option", b =>
                {
                    b.HasOne("AxeAssessmentToolWebAPI.Models.Question", "Question")
                        .WithMany("Options")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.Question", b =>
                {
                    b.HasOne("AxeAssessmentToolWebAPI.Models.QuestionType", "QuestionType")
                        .WithMany("Question")
                        .HasForeignKey("QuestionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuestionType");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.UserTest", b =>
                {
                    b.HasOne("AxeAssessmentToolWebAPI.Models.User", "User")
                        .WithOne("UserTest")
                        .HasForeignKey("AxeAssessmentToolWebAPI.Models.UserTest", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.User_OptionSelected", b =>
                {
                    b.HasOne("AxeAssessmentToolWebAPI.Models.User_QuestionAttempted", "QuestionAttempted")
                        .WithMany("OptionSelected")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuestionAttempted");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.User_QuestionAttempted", b =>
                {
                    b.HasOne("AxeAssessmentToolWebAPI.Models.User", "User")
                        .WithMany("QuestionAttempted")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.Question", b =>
                {
                    b.Navigation("Options");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.QuestionType", b =>
                {
                    b.Navigation("Question");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.User", b =>
                {
                    b.Navigation("QuestionAttempted");

                    b.Navigation("UserTest")
                        .IsRequired();
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.User_QuestionAttempted", b =>
                {
                    b.Navigation("OptionSelected");
                });
#pragma warning restore 612, 618
        }
    }
}
