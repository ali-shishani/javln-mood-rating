using InterviewProjectTemplate.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InterviewProjectTemplate.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        private readonly CustomWebAppOptions _webAppOptions;
        private readonly IAuthenticatedAccountService _authenticatedAccountService;

        public AccountController(IAccountService accountService,
                        IAuthenticatedAccountService authenticatedAccountService,
                        ILogger<AccountController> logger,
                        IOptions<CustomWebAppOptions> webAppOptions)
        {
            _accountService = accountService;
            _authenticatedAccountService = authenticatedAccountService;
            _logger = logger;
            _webAppOptions = webAppOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost("user/authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            _logger.LogInformation("User trying to login:{Email}", request.Email);
            return Ok(await _accountService.AuthenticateAsync(request));

        }

        [HttpPost("user/refresh-token")]
        public async Task<ActionResult<UserDetailsResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            _logger.LogInformation("Attempting to refresh token for email: {Email}.", request.Email);
            return Ok(await _accountService.RefreshTokenAsync(request));
        }

        [HttpGet("user/logout")]
        public async Task<ActionResult<bool>> LogoutAsync(string userEmail)
        {
            _logger.LogInformation("Attempting to logout user: {userEmail}.", userEmail);
            return Ok(await _accountService.LogoutAsync(userEmail));
        }

        [Authorize]
        [HttpGet("user/claims")]
        public async Task<ActionResult<IDictionary<string, string>>> GetCurrentUserClaimsForPath(string claimPath)
        {
            var usrName = User.Identity.Name;
            var claims = await _accountService.GetUserClaims(usrName, claimPath);
            return Ok(claims);
        }

        [Authorize]
        [HttpGet("user/profile")]
        public async Task<ActionResult<UserProfileResponse>> GetUserProfileDetails()
        {
            var result = await _authenticatedAccountService.GetAccountUserProfile();
            return Ok(result);
        }

        private string GenerateIPAddress()
        {
            _logger.LogInformation($"Attempting to fetch the Ip address for current request.");
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

    }
}
