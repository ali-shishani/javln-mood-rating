using InterviewProjectTemplate.Config.Provider;
using InterviewProjectTemplate.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data
{
    public class MoodRatingDbContext: DbContext
    {
        static string connectionString = "mysql-db;Port=3306;Database=moodtrackerdb;Uid=app;Pwd=password";
        private readonly IAppConfigurationProvider _appConfigurationProvider;

        public MoodRatingDbContext(IAppConfigurationProvider appConfigurationProvider) : base()
        {
            _appConfigurationProvider = appConfigurationProvider;
            connectionString = _appConfigurationProvider.GetConnectionString();
        }

        // add your entities here
        public DbSet<MoodRatingRecord> MoodRatingRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
