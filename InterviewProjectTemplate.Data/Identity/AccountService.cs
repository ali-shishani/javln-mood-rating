using InterviewProjectTemplate.Config.Model;
using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        //private readonly IdentityContext _identityDbContext;
        private readonly MoodRatingDbContext _identityDbContext;

        private readonly MoodRatingDbContext _dbContext;
        private readonly ILogger<AccountService> _logger;

        private readonly IAccountSignInService _accountSignInService;
        private readonly IEmailSender<ApplicationUser> _emailSender;


        public int DefaultLimit { get; set; } = 10;
        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            //IdentityContext identityDbContext,
            MoodRatingDbContext identityDbContext,
            MoodRatingDbContext dbContext,
            IEmailSender<ApplicationUser> emailSender,
            ILogger<AccountService> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _identityDbContext = identityDbContext;
            _dbContext = dbContext;
            _logger = logger;
            _emailSender = emailSender;
            _accountSignInService = new AccountSignInService(userManager, signInManager,
                                                    roleManager, _jwtSettings, logger);
        }

        async Task<ApplicationUser> FindRequiredUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User {userId} not found.", userId);
                throw new AccountException(AccountErrorCode.LOGIN_USER_NOT_FOUND,
                    $"User {userId} not found.", (int)HttpStatusCode.NotFound);
            }
            return user;
        }

        async Task<ApplicationUser> FindRequiredUserByEmail(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User with email {email} not found.", email);
                throw new AccountException(AccountErrorCode.LOGIN_USER_NOT_FOUND,
                    $"User not found for {email}");
            }

            return user;
        }


        /// <summary>
        /// TODO: 2024-02-04 experiemental - slow, will need caching
        /// </summary>
        /// <param name="usrName"></param>
        /// <param name="claimPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IDictionary<string, string>> GetUserClaims(string usrName, string claimPath)
        {
            var user = await _userManager.FindByNameAsync(usrName);
            if (user == null)
            {
                throw new Exception("Can not get users claims");
            }

            var allClaims = await _userManager.GetClaimsAsync(user);


            var roleIds = await _dbContext.UserRoles.Where(x => x.UserId == user.Id)
                .Select(x => x.RoleId).ToListAsync();

            var claimsByRoles = await _dbContext.RoleClaims
                                                .Where(x => roleIds.Contains(x.RoleId))
                                                .Select(x => new
                                                {
                                                    ClaimType = x.ClaimType,
                                                    ClaimValue = x.ClaimValue,
                                                })
                                                .ToListAsync();

            foreach (var applicationRoleClaim in claimsByRoles)
            {
                var name = applicationRoleClaim.ClaimType;
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("name is null");
                }

                if (name.Contains("Permission", StringComparison.InvariantCultureIgnoreCase))
                {
                    name = name + "_" + Guid.NewGuid();
                }

                allClaims.Add(new Claim(name, applicationRoleClaim.ClaimValue));
            }
            return allClaims.ToDictionary(x => x.Type, y => y.Value);
        }

        public async Task<bool> LogoutAsync(string userEmail)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation($"User with email {userEmail} logged out  successfully.");
            return true;
        }

        #region AccountSignIn
        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
            => await _accountSignInService.AuthenticateAsync(request);

        public async Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request)
            => await _accountSignInService.RefreshTokenAsync(request);

        public async Task<AuthenticationResponse> VerifyMFACode(ValidateMfaToken request)
            => await _accountSignInService.VerifyMFACode(request);

        #endregion

    }
}
