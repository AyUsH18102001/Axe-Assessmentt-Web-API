using Microsoft.EntityFrameworkCore;

namespace AxeAssessmentToolWebAPI.Data
{
    public class DataContext2 : DbContext
    {
        public DataContext2(DbContextOptions<DataContext2> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.Development.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("connectionString2"));
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public DbSet<University> University { get; set; }

    }
}
