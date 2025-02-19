using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    public interface IAuthenticatedUserProvider
    {
        AuthenticatedUserModel GetCurrentUser();
    }

    public interface IAuthenticatedUserSetter
    {
        void SetUser(AuthenticatedUserModel user);
    }

    public record AuthenticatedUserModel(Guid UserId,
                                        string UserName,
                                        string Email,
                                        IEnumerable<string>? Roles = null);

    /// <summary>
    /// Typically we inject IAuthenticatedUserProvider (which wont change user)
    /// </summary>
    public class AuthenticatedUserProvider : IAuthenticatedUserProvider,
                                             IAuthenticatedUserSetter
    {
        private readonly IHttpContextAccessor _httpCtx;

        private AuthenticatedUserModel? _user;

        public AuthenticatedUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpCtx = httpContextAccessor;
        }

        public AuthenticatedUserModel GetCurrentUser()
        {
            // If user set elsewhere
            if (_user != null)
                return _user;

            if (_httpCtx.HttpContext == null)
                return null!;

            var user = _httpCtx.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
                return null;

            var sidClaim = user.FindFirstValue(JwtRegisteredClaimNames.Sid);
            var sid = Guid.Parse(sidClaim!);
            var usernameClaim = user.FindFirstValue(ClaimTypes.Name);
            var emailClaim = user.FindFirstValue(ClaimTypes.Email);
            var roles = user.Claims
                            .Where(x => x.Type == ClaimTypes.Role)
                            .Select(r => r.Value);

            return new AuthenticatedUserModel(sid, usernameClaim!, emailClaim!, roles);
        }

        /// <summary>
        /// Manually set user for scoped provider
        /// </summary>
        /// <param name="user"></param>
        public void SetUser(AuthenticatedUserModel user)
        {
            _user = user;
        }
    }
}
