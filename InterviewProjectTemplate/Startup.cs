﻿using InterviewProjectTemplate.Services.Mood;
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
            // TODO: Register repositories
        }
    }
}
