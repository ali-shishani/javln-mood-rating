using InterviewProjectTemplate.Config.Provider;
using InterviewProjectTemplate.Data;
using InterviewProjectTemplate.Data.Entity;
using InterviewProjectTemplate.Repositories;
using InterviewProjectTemplate.Services.Mood;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace InterviewProjectTemplate
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<Startup> _logger;
        private readonly IWebHostEnvironment _hostEnv;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnv = hostEnvironment;

            var loggerFactory = LoggerFactory.Create(builder =>
                builder.AddSimpleConsole(options => options.SingleLine = true));
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: implement config
            //var provider = services.BuildServiceProvider();

            // Add services to the container.
            services.AddSingleton<IAppConfigurationProvider, AppConfigurationProvider>();
            services.AddScoped<MoodRatingDbContext>();
            services.AddDbContext<MoodRatingDbContext>();

            // add authenication
            services.AddAuthentication()
                .AddCookie(IdentityConstants.ApplicationScheme)
                .AddBearerToken(IdentityConstants.BearerScheme);

            // add authorisation
            services.AddAuthorization();

            // add identity core
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<MoodRatingDbContext>()
                .AddApiEndpoints();

            services.AddCors(o => o.AddDefaultPolicy(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()));

            RegisterRepositories(services);
            RegisterServices(services);

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private IServiceCollection RegisterServices(IServiceCollection services)
        {
            // register services
            services.AddTransient<IMoodRatingService, MoodRatingService>();

            return services;
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            // register repositories
            services.AddScoped<IMoodRatingRecordRepository, MoodRatingRecordRepository>();
        }
    }
}
