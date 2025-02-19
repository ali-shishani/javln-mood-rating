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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using Duende.IdentityServer.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.EntityFramework.Stores;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Duende.IdentityServer.EntityFramework.Storage;
using MySqlX.XDevAPI;

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
                settings.DurationInMinutes = jwtSettings.DurationInMinutes;
            });

            //services.AddInMemoryIdentityResources(IdentityConfiguration.GetIdentityResources())
            //services.AddInMemoryClients(IdentityConfiguration.GetClients(builder.Configuration))

            //services.AddInMemoryIdentityResources(configuration.GetIdentityResources());
            //services.AddInMemoryClients(jwtSettings.GetClients(configuration));

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthenticatedAccountService, AuthenticatedAccountService>();
            services.AddTransient<IUserAccountService, UserAccountService>();

            services.AddTransient<IEmailSender<ApplicationUser>, CustomEmailSender>();
            services.AddTransient<IClientStore, ClientStore>();
            services.AddTransient<IdentityServerOptions>();
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
            })
               .AddEntityFrameworkStores<MoodRatingDbContext>()
               .AddDefaultTokenProviders()
               .AddTokenProvider(jwtSettings.Issuer, typeof(DataProtectorTokenProvider<ApplicationUser>))
               .AddUserManager<CustomUserManager>();

            services.AddIdentityServer()
                .AddInMemoryPersistedGrants()
                //.AddInMemoryIdentityResources(configuration.GetIdentityResources())
                //.AddInMemoryApiResources(configuration.GetApiResources())
                //.AddInMemoryClients(configuration.GetClients())
                .AddAspNetIdentity<ApplicationUser>();

            services.AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseMySQL(conn);
            });

            // TODO: enable identity after fixing https setup in front-end
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(o =>
            //{
            //    o.RequireHttpsMetadata = false;
            //    o.SaveToken = false;
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.Zero,
            //        ValidIssuer = jwtSettings.Issuer,
            //        ValidAudience = jwtSettings.Audience,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            //    };
            //    o.Events = new JwtBearerEvents()
            //    {
            //        OnAuthenticationFailed = c =>
            //        {
            //            c.NoResult();
            //            c.Response.StatusCode = 500;
            //            c.Response.ContentType = "application/json";
            //            var probDetails = new ProblemDetails()
            //            {
            //                Type = c.Exception.GetType().Name,
            //                Title = "AuthFailed",
            //                Detail = c.Exception.Message,
            //                Status = (int)HttpStatusCode.InternalServerError,
            //                Instance = c.Request.GetDisplayUrl()
            //            };
            //            var res = JsonHelpers.SerializeJson(probDetails);
            //            return c.Response.WriteAsync(res);
            //        },
            //        OnChallenge = context =>
            //        {
            //            context.HandleResponse();
            //            context.Response.StatusCode = 401;
            //            context.Response.ContentType = "application/json";
            //            var result = JsonHelpers.SerializeJson(new ProblemDetails()
            //            {
            //                Title = "Unauthorized",
            //                Detail = "You are not authorized",
            //                Status = (int)HttpStatusCode.Unauthorized
            //            });
            //            return context.Response.WriteAsync(result);
            //        },
            //        OnForbidden = context =>
            //        {
            //            context.Response.StatusCode = 403;
            //            context.Response.ContentType = "application/json";
            //            var result = JsonHelpers.SerializeJson(new ProblemDetails()
            //            {
            //                Title = "Forbidden",
            //                Detail = "You are not authorized to this resource",
            //                Status = (int)HttpStatusCode.Forbidden
            //            });
            //            return context.Response.WriteAsync(result);
            //        },
            //    };
            //})
            //.AddOpenIdConnect(options =>
            //{
            //    var oidcConfig = configuration.GetSection("OpenIDConnectSettings");

            //    options.Authority = oidcConfig["Authority"];
            //    options.ClientId = oidcConfig["ClientId"];
            //    options.ClientSecret = oidcConfig["ClientSecret"];

            //    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.ResponseType = OpenIdConnectResponseType.Code;

            //    options.SaveTokens = true;
            //    options.GetClaimsFromUserInfoEndpoint = true;

            //    options.MapInboundClaims = false;
            //    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
            //    options.TokenValidationParameters.RoleClaimType = "roles";
            //});
        }


        public static IServiceCollection AddAuthenticatedUserProvider(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticatedUserProvider, AuthenticatedUserProvider>();
            services.AddScoped<IAuthenticatedUserSetter>(provider =>
                (AuthenticatedUserProvider)provider.GetService<IAuthenticatedUserProvider>());

            return services;
        }

        public static IServiceCollection AddConfigurationStore(this IServiceCollection services,
        Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            return services.AddConfigurationStore<ConfigurationDbContext>(storeOptionsAction);
        }

        public static IServiceCollection AddConfigurationStore<TContext>(this IServiceCollection services,
        Action<ConfigurationStoreOptions> storeOptionsAction = null)
        where TContext : DbContext, IConfigurationDbContext
        {
            var options = new ConfigurationStoreOptions();
            services.AddSingleton(options);
            storeOptionsAction?.Invoke(options);

            if (options.ResolveDbContextOptions != null)
            {
                services.AddDbContext<TContext>(options.ResolveDbContextOptions);
            }
            else
            {
                services.AddDbContext<TContext>(dbCtxBuilder =>
                {
                    options.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }
            //services.AddScoped<IConfigurationDbContext, TContext>();

            services.AddScoped<IConfigurationDbContext, ConfigurationDbContext>();
            services.AddScoped<IResourceStore, ResourceStore>();

            return services;
        }
    }
}
