using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace AxeAssessmentToolWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {  
        }

        public DataContext()
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

            modelBuilder.Entity<Question>().HasIndex(u => u.QuestionContent).IsUnique(); // make questionContent unique
            modelBuilder.Entity<QuestionType>().HasIndex(qt => qt.short_name).IsUnique(); // make questioType short_name unique

            modelBuilder.Entity<SQL_Question>().Navigation(sql_q => sql_q.SQL_Answer).AutoInclude();  // include the sqlAnswer along with the sqlQuestion

            /*modelBuilder.Entity<Test>().Navigation(t => t.TestQuestions).AutoInclude();  // include the test qustions along with the test
            modelBuilder.Entity<Test>().Navigation(t => t.TestRules).AutoInclude();  // include the test rules along with the test*/

            modelBuilder.Entity<User>().Navigation(u => u.UserTest).AutoInclude();   // include the userTest along with the user
            modelBuilder.Entity<User>().Navigation(u => u.QuestionAttempted).AutoInclude(); // include the userTest along with the user

            modelBuilder.Entity<Option>()
                .HasOne(op => op.Question)
                .WithMany(q => q.Options)
            .HasForeignKey("QuestionId");

            modelBuilder.Entity<Question>()
                .HasOne(q => q.QuestionType)
                .WithMany(qt => qt.Question)
            .HasForeignKey("short_name");

            modelBuilder.Entity<Question>()
                .HasOne(q => q.TestType)
                .WithMany(tt => tt.Questions)
            .HasForeignKey("TestTypeId");

            modelBuilder.Entity<User_QuestionAttempted>()
                .HasOne(u_qa => u_qa.User)
                .WithMany(u => u.QuestionAttempted)
            .HasForeignKey("UserId");

            modelBuilder.Entity<SQL_Answer>()
                .HasOne(sql_a => sql_a.SQL_Question)
                .WithOne(sql_q => sql_q.SQL_Answer)
            .HasForeignKey<SQL_Answer>("QuestionId");

            modelBuilder.Entity<TestQuestions>()
                .HasOne(tq => tq.Test)
                .WithMany(t => t.TestQuestions)
           .HasForeignKey("TestId");

            modelBuilder.Entity<TestRules>()
                .HasOne(tr => tr.Test)
                .WithMany(t => t.TestRules)
           .HasForeignKey("TestId");
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTest> UserTests { get; set; }
        public DbSet<QuestionType> QuestionType { get; set; }
        public DbSet<TestType> TestType { get; set; }
        public DbSet<User_QuestionAttempted> User_QuestionAttempted { get; set; }
        public DbSet<SQL_Question> SQL_Question { get; set; }
        public DbSet<SQL_Answer> SQL_Answer { get; set; }
        public DbSet<Rules> Rules { get; set; }
        public DbSet<TestRules> TestRules { get; set; }
        public DbSet<TestQuestions> TestQuestions { get; set; }
        public DbSet<Test> Test { get; set; }   

    }
}
