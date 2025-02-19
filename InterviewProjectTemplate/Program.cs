
using Microsoft.EntityFrameworkCore;
using InterviewProjectTemplate.Config.Provider;
using InterviewProjectTemplate.Data.Entity;
using InterviewProjectTemplate.Seeds;
using Microsoft.Extensions.Logging;

namespace InterviewProjectTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var hostEnv = builder.Environment;
            var configuration = builder.Configuration;

            configuration
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            var startup = new Startup(configuration, hostEnv);
            startup.ConfigureServices(builder.Services);
            var app = builder.Build();

            var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Environment: {Env}", hostEnv.EnvironmentName);

            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                try
                {
                    logger.LogInformation("Start Seeding Data");
                    SeedData.Seed(services, hostEnv.EnvironmentName);
                    logger.LogInformation("Seeding of Data Completed");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed database seed - {Message}", ex.GetBaseException().Message);
                    throw;
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();

            //app.UseAuthorization(); TODO: clean-up / this is moved to startup.
            app.MapIdentityApi<ApplicationUser>();

            app.MapControllers();

            app.Run();
        }
    }
}
