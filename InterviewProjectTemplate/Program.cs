
using Microsoft.EntityFrameworkCore;
using InterviewProjectTemplate.Config.Provider;
using InterviewProjectTemplate.Data.Entity;

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
