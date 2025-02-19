using InterviewProjectTemplate.Data.Entity;
using InterviewProjectTemplate.Data.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using InterviewProjectTemplate.Data;

namespace InterviewProjectTemplate.Seeds
{
    public static class SeedData
    {
        public static void Seed(IServiceProvider services, string hostEnvironmentName)
        {
            var dbContext = services.GetRequiredService<MoodRatingDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(SeedData));

            var nowUtc = DateTime.UtcNow;
            var stopWatch = new Stopwatch();

            logger.LogInformation("SeedData-Begin: {NowUtc}", nowUtc.ToString("T"));
            stopWatch.Start();

            // users
            DefaultRoles.Seed(roleManager);
            DefaultAdmin.Seed(userManager);

            stopWatch.Stop();
            logger.LogInformation("SeedData-End: {NowUtc}  TotalDurationSeconds: {ElapsedSeconds}",
                nowUtc.ToString("T"), stopWatch.ElapsedMilliseconds / 60);
        }

        private static bool IsProductionEnvironment(string environment)
        {
            return environment.Contains("Prod", StringComparison.InvariantCultureIgnoreCase)
                   || environment.Contains("live", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
