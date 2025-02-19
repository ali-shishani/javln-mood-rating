using InterviewProjectTemplate.Config.Provider;
using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data
{
    public class MoodRatingDbContext
    : IdentityDbContext<
    ApplicationUser, ApplicationRole, Guid,
    ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
    ApplicationRoleClaim, ApplicationUserToken>
    {
        static string connectionString = "";
        private readonly IAppConfigurationProvider _appConfigurationProvider;

        public MoodRatingDbContext(IAppConfigurationProvider appConfigurationProvider) : base()
        {
            _appConfigurationProvider = appConfigurationProvider;
            connectionString = _appConfigurationProvider.GetConnectionString();
        }

        // add your entities here
        public virtual DbSet<UserDetail> UserDetails { get; set; }
        public virtual DbSet<MoodRatingRecord> MoodRatingRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString, mySqlOptions =>
            {
                // TODO: wire these options in appsettings.json
                mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,  // Adjust the maximum number of retry attempts as needed
                    maxRetryDelay: TimeSpan.FromSeconds(30),  // Adjust the maximum delay between retries as needed
                    errorNumbersToAdd: null);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
