using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace InterviewProjectTemplate.AuthHandler
{
    public class CustomAuthHandler : AuthorizationHandler<CustomRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
        {
            // user security id should be userid
            var userSid = context.User.FindFirst(x => x.Type == ClaimTypes.Sid);

            if (context.Resource is HttpContext httpContext)
            {
                // You can customise logic like below

                //var endpoint = httpContext.GetEndpoint();
                //var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

                //var routeCustomId = httpContext?.GetRouteValue("customId") as string;

                //if (!string.IsNullOrEmpty(routeCustomId))
                //{
                //    var propertyId = Convert.ToInt32(routeCustomId);
                //}
            }

            if (context.Resource is AuthorizationFilterContext mvcContext)
            {
                // You can customise logic like below

                //// Examine MVC-specific things like routing data.
                //var authContext = (AuthorizationFilterContext)context.Resource;
                //var routeCustomId1 = authContext?.HttpContext?.GetRouteValue("customId") as int?;

            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
