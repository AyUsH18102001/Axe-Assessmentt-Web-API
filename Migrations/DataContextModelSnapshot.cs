﻿// <auto-generated />
using System;
using AxeAssessmentToolWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AxeAssessmentToolWebAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("QuestionsType")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");

                    b.HasData(
                        new
                        {
                            QuestionId = 3,
                            QuestionContent = "Is HTML a Programming Language",
                            QuestionsType = "True/False"
                        },
                        new
                        {
                            QuestionId = 4,
                            QuestionContent = "What is T-SQL",
                            QuestionsType = "MCQ"
                        },
                        new
                        {
                            QuestionId = 5,
                            QuestionContent = "Select all the Logical Operators",
                            QuestionsType = "Multiple Select"
                        },
                        new
                        {
                            QuestionId = 6,
                            QuestionContent = "What is a sub query",
                            QuestionsType = "MCQ"
                        },
                        new
                        {
                            QuestionId = 7,
                            QuestionContent = "Select the correct query",
                            QuestionsType = "MCQ"
                        },
                        new
                        {
                            QuestionId = 8,
                            QuestionContent = "Which keyword is used for aliasing",
                            QuestionsType = "MCQ"
                        },
                        new
                        {
                            QuestionId = 9,
                            QuestionContent = "Can a Table have two Primary Keys",
                            QuestionsType = "True/False"
                        },
                        new
                        {
                            QuestionId = 10,
                            QuestionContent = "What is View",
                            QuestionsType = "MCQ"
                        });
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

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

                    b.Property<string>("UserResumeFileName")
                        .HasColumnType("nvarchar(150)");

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

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.Option", b =>
                {
                    b.HasOne("AxeAssessmentToolWebAPI.Models.Question", "Question")
                        .WithMany("Options")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
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

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.Question", b =>
                {
                    b.Navigation("Options");
                });

            modelBuilder.Entity("AxeAssessmentToolWebAPI.Models.User", b =>
                {
                    b.Navigation("UserTest")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
