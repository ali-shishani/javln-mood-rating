using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using InterviewProjectTemplate.Data;
using Microsoft.EntityFrameworkCore;
using InterviewProjectTemplate.Config.Model;
using InterviewProjectTemplate.Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace InterviewProjectTemplate.Extensions
{
    public static class IdentityServiceExtensions
    {
        private const string JwtSettingsSection = "JWTSettings";
        private const string IdentityConnectionStringKey = "MySQLConnectionString";


        public static void AddCustomIdentity(this IServiceCollection services,
                                            IConfiguration configuration,
                                            IHostEnvironment hostEnvironment)
        {
            JwtSettings jwtSettings = configuration.GetSection(JwtSettingsSection)
                                            .Get<JwtSettings>()!;

            services.Configure<JwtSettings>(settings =>
            {
                settings.Audience = jwtSettings.Audience;
                settings.Issuer = jwtSettings.Issuer;
                settings.Key = jwtSettings.Key;
                settings.Issuer = jwtSettings.Issuer;
                settings.DurationInMinutes = jwtSettings.DurationInMinutes;
            });

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthenticatedAccountService, AuthenticatedAccountService>();
            services.AddTransient<IUserAccountService, UserAccountService>();

            services.AddTransient<IEmailSender, CustomEmailSender>();
            services.AddTransient<RoleManager<ApplicationRole>>();

            var conn = configuration.GetConnectionString(IdentityConnectionStringKey);

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.ChangePhoneNumberTokenProvider = TokenOptions.DefaultPhoneProvider;
                options.Tokens.ChangePhoneNumberTokenProvider = TokenOptions.DefaultPhoneProvider;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
            }).AddEntityFrameworkStores<MoodRatingDbContext>()
               .AddDefaultTokenProviders()
               .AddTokenProvider("MyApp", typeof(DataProtectorTokenProvider<ApplicationUser>))
               .AddUserManager<CustomUserManager>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(o =>
               {
                   o.RequireHttpsMetadata = false;
                   o.SaveToken = false;
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,
                       ValidIssuer = jwtSettings.Issuer,
                       ValidAudience = jwtSettings.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                   };
                   o.Events = new JwtBearerEvents()
                   {
                       OnAuthenticationFailed = c =>
                       {
                           c.NoResult();
                           c.Response.StatusCode = 500;
                           c.Response.ContentType = "application/json";
                           var probDetails = new ProblemDetails()
                           {
                               Type = c.Exception.GetType().Name,
                               Title = "AuthFailed",
                               Detail = c.Exception.Message,
                               Status = (int)HttpStatusCode.InternalServerError,
                               Instance = c.Request.GetDisplayUrl()
                           };
                           var res = JsonHelpers.SerializeJson(probDetails);
                           return c.Response.WriteAsync(res);
                       },
                       OnChallenge = context =>
                       {
                           context.HandleResponse();
                           context.Response.StatusCode = 401;
                           context.Response.ContentType = "application/json";
                           var result = JsonHelpers.SerializeJson(new ProblemDetails()
                           {
                               Title = "Unauthorized",
                               Detail = "You are not authorized",
                               Status = (int)HttpStatusCode.Unauthorized
                           });
                           return context.Response.WriteAsync(result);
                       },
                       OnForbidden = context =>
                       {
                           context.Response.StatusCode = 403;
                           context.Response.ContentType = "application/json";
                           var result = JsonHelpers.SerializeJson(new ProblemDetails()
                           {
                               Title = "Forbidden",
                               Detail = "You are not authorized to this resource",
                               Status = (int)HttpStatusCode.Forbidden
                           });
                           return context.Response.WriteAsync(result);
                       },
                   };
               });
        }


        public static IServiceCollection AddAuthenticatedUserProvider(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticatedUserProvider, AuthenticatedUserProvider>();
            services.AddScoped<IAuthenticatedUserSetter>(provider =>
                (AuthenticatedUserProvider)provider.GetService<IAuthenticatedUserProvider>());

            return services;
        }
    }
}
