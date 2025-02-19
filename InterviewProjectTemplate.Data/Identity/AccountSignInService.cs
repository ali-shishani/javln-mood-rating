using InterviewProjectTemplate.Config.Model;
using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InterviewProjectTemplate.Data.Identity
{
    public record AccountAuthenticatedEvent(Guid UserId, string Email, string? IpAddress);


    public interface IAccountSignInService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<AuthenticationResponse> VerifyMFACode(ValidateMfaToken request);
    }

    public class AccountSignInService : IAccountSignInService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        //private readonly IEventPublisher _eventPublisher;

        public AccountSignInService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            JwtSettings jwtSettings,
            //IEventPublisher eventPublisher,
            ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _roleManager = roleManager;
            _logger = logger;
            //_eventPublisher = eventPublisher;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email.Trim());

            if (user == null)
            {
                _logger.LogWarning($"User not registered for '{request.Email}'.");
                throw new AccountException(AccountErrorCode.LOGIN_EMAIL_NOT_REGISTERED,
                    "email not registered", nameof(request.Email), $"{request.Email} not registered");
            }
            if (!user.EmailConfirmed)
            {
                _logger.LogWarning($"Account not confirmed for '{request.Email}'.");
                throw new AccountException(AccountErrorCode.LOGIN_EMAIL_NOT_CONFIRMED,
                    "email not confirmed", nameof(request.Email), $"{request.Email} not confirmed");
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(
                                                                        user: user,
                                                                        password: request.Password,
                                                                        isPersistent: false,
                                                                        lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                if (signInResult.RequiresTwoFactor
                    && (await _userManager.GetTwoFactorEnabledAsync(user)))
                {
                    var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

                    if (!string.IsNullOrEmpty(request.Code))
                    {
                        var code = request.Code
                            .Replace(" ", string.Empty)
                            .Replace("-", string.Empty);

                        signInResult = await _signInManager.TwoFactorAuthenticatorSignInAsync(
                            code: code,
                            isPersistent: false,
                            rememberClient: request.RememberMe);

                        if (!signInResult.Succeeded)
                        {
                            throw new AccountException(AccountErrorCode.LOGIN_TWOFACTOR_CODE_INVALID,
                                "2 factor invalid", nameof(request.Code), "invalid 2fa");
                        }
                    }

                    // TODO: 2024-02-24 for SMS only
                    //        string code = await _userManager.GenerateUserTokenAsync(user, "Phone", "MFA");
                    //        await _accountService.SendSmsCodeToPhoneAsync(user, code);
                    //        _logger.LogInformation($"Need 2FA for {request.Email} .");

                    if (!signInResult.Succeeded)
                    {
                        throw new AccountException(AccountErrorCode.LOGIN_TWOFACTOR_CODE_REQUIRED,
                            "2fa required", nameof(request.Code), "Two-factor authentication required code");
                    }
                }
                else
                {
                    _logger.LogWarning($"Invalid credentials for '{request.Email}'.");
                    throw new AccountException(AccountErrorCode.LOGIN_INVALID_CREDENTIALS, "InvalidCredentials");
                }
            }

            var response = await CreateAuthenticationResponseWithJwt(user, true);
            //await _eventPublisher.Publish(new AccountAuthenticatedEvent(Guid.Parse(response.Id), response.Email, ""));
            _logger.LogInformation($"Authenticated {request.Email} .");
            return response;
        }

        public async Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning($"User is not registered with '{request.Email}'.");
                throw new AccountException(AccountErrorCode.LOGIN_EMAIL_NOT_REGISTERED, $"You are not registered with {request.Email} ");
            }
            if (!user.EmailConfirmed)
            {
                _logger.LogWarning($"Account Not Confirmed for '{request.Email}'.");
                throw new AccountException(AccountErrorCode.LOGIN_EMAIL_NOT_CONFIRMED,
                    $"{request.Email} not confirmed");
            }

            string? refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            if (refreshToken == null)
                throw new Exception("failed to retrieve refresh token");

            bool isValid = await _userManager.VerifyUserTokenAsync(user, "MyApp", "RefreshToken", request.Token);
            if (!refreshToken.Equals(request.Token) || !isValid)
            {
                _logger.LogWarning($"Your token is not valid.");
                throw new AccountException(AccountErrorCode.LOGIN_REFRESH_TOKEN_INVALID, "Token not valid");
            }

            string ipAddress = IpHelper.GetIpAddress();
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, ipAddress);
            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user.Id.ToString(),
                JwToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email!,
                UserName = user.UserName!
            };
            IList<string> rolesList = await _userManager.GetRolesAsync(user);
            IList<RoleInfo> roles = new List<RoleInfo>();

            foreach (string item in rolesList)
            {
                var role = await _roleManager.FindByNameAsync(item);
                if (role != null)
                {
                    roles.Add(new RoleInfo { RoleId = role.Id, RoleName = role.Name });
                }
            }
            response.RolesInfo = roles.ToList();
            // response.Roles = rolesList.ToList();
            response.IsEmailVerified = user.EmailConfirmed;
            response.RefreshToken = await GenerateRefreshToken(user);

            await _signInManager.SignInAsync(user, false);
            _logger.LogInformation($"Authenticated '{user.UserName}'.");
            return response;
        }

        private async Task<AuthenticationResponse> CreateAuthenticationResponseWithJwt(ApplicationUser user, bool canGenerateJwt = false)
        {
            AuthenticationResponse response = new()
            {
                Id = user.Id.ToString(),
                Email = user.Email!,
                UserName = user.UserName!,
                //FirstName = user.FirstName,
                //LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsEmailVerified = user.EmailConfirmed,
                IsPhoneVerified = user.PhoneNumberConfirmed,
                HasAuthenticator = (await _userManager.GetAuthenticatorKeyAsync(user) != null),
                TwoFactorEnabled = user.TwoFactorEnabled,
                TwoFactorClientRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
            };
            if (canGenerateJwt)
            {
                _logger.LogInformation("JWT token generation possible for the user.");
                string ipAddress = IpHelper.GetIpAddress();
                JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, ipAddress);
                response.JwToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                response.RefreshToken = await GenerateRefreshToken(user);
                IList<string> rolesList = await _userManager.GetRolesAsync(user);
                IList<RoleInfo> roles = new List<RoleInfo>();

                foreach (string item in rolesList)
                {
                    // TODO: 2024-02-24 not sure if this is hitting db query for each role.... if so can refactor, or cache roles - rs
                    ApplicationRole? role = await _roleManager.FindByNameAsync(item);
                    if (role != null)
                    {
                        roles.Add(new RoleInfo { RoleId = role.Id, RoleName = role.Name });
                    }
                }
                response.RolesInfo = roles.ToList();
            }

            return response;
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user, string ipAddress)
        {
            _logger.LogInformation($"Generating JWT token for user {user.Id}.");
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            List<Claim> roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            IEnumerable<Claim> claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                    new Claim("ip", ipAddress)
                }
                .Union(userClaims)
                .Union(roleClaims);

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            _logger.LogInformation("JWT token generated successfully for user {Id}.", user.Id);
            return jwtSecurityToken;
        }

        /// <summary>
        /// TODO: 2024-02-24 need to review this as its currently tied to single provider
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthenticationResponse> VerifyMFACode(ValidateMfaToken request)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                _logger.LogWarning($"User not registered for '{request.Id}'.");
                throw new AccountException(AccountErrorCode.LOGIN_USER_NOT_REGISTERED,
                    $"You are not registered with '{request.Id}'.");
            }
            if (!(await _userManager.VerifyUserTokenAsync(user, "Phone", "MFA", request.Code)))
            {
                _logger.LogWarning($"Invalid Credentials for '{user.Email}'.");
                throw new AccountException(AccountErrorCode.LOGIN_INVALID_CREDENTIALS,
                    $"Invalid credentials for {user.Email}");
            }

            await _userManager.RemoveAuthenticationTokenAsync(user, "Phone", "MFA");
            AuthenticationResponse response = await CreateAuthenticationResponseWithJwt(user, true);
            _logger.LogInformation($"Authenticated {user.Email} .");
            return response;
        }

        /// <summary>
        /// TODO: 2024-02-24 needs review
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<string> GenerateRefreshToken(ApplicationUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            string newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "MyApp", "RefreshToken");
            IdentityResult result = await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", newRefreshToken);
            if (!result.Succeeded)
            {
                throw new AccountException(AccountErrorCode.LOGIN_REFRESH_TOKEN_GENERATION_FAILED,
                    "Unable to set refresh token");
            }

            _logger.LogInformation($"Refresh token generated successfully for user {user.Id}.");
            return newRefreshToken;
        }
    }
}
