using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace AxeAssessmentToolWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {  
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.Development.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("connectionString"));
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>().Navigation(q => q.Options).AutoInclude();   // include the options along with the questions
            modelBuilder.Entity<User>().Navigation(u => u.UserTest).AutoInclude();   // include the userTest along with the user

            modelBuilder.Entity<Option>()
                .HasOne(op => op.Question)
                .WithMany(q => q.Options)
            .HasForeignKey("QuestionId");


            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    QuestionId = 3,
                    QuestionContent = "Is HTML a Programming Language",
                    QuestionsType = "True/False",
                },
                new Question
                {
                    QuestionId = 4,
                    QuestionContent = "What is T-SQL",
                    QuestionsType = "MCQ",
                },
                new Question
                {
                    QuestionId = 5,
                    QuestionContent = "Select all the Logical Operators",
                    QuestionsType = "Multiple Select",
                },
                new Question
                {
                    QuestionId = 6,
                    QuestionContent = "What is a sub query",
                    QuestionsType = "MCQ",
                },
                new Question
                {
                    QuestionId = 7,
                    QuestionContent = "Select the correct query",
                    QuestionsType = "MCQ",
                },
                new Question
                {
                    QuestionId = 8,
                    QuestionContent = "Which keyword is used for aliasing",
                    QuestionsType = "MCQ",
                },
                new Question
                {
                    QuestionId = 9,
                    QuestionContent = "Can a Table have two Primary Keys",
                    QuestionsType = "True/False",
                },
                new Question
                {
                    QuestionId = 10,
                    QuestionContent = "What is View",
                    QuestionsType = "MCQ",
                }
            );

        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTest> UserTests { get; set; }
    }
}
