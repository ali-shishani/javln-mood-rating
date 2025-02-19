using Microsoft.AspNetCore.Authorization;

namespace InterviewProjectTemplate.AuthHandler
{
    public static class AuthHandlerServiceRegistration
    {
        public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
        {
            // see: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-8.0#handler-registration
            services.AddSingleton<IAuthorizationHandler, CustomAuthHandler>();

            return services;
        }
    }
}
