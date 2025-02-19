using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> VerifyMFACode(ValidateMfaToken request);
        Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<bool> LogoutAsync(string userEmail);
        Task<IDictionary<string, string>> GetUserClaims(string usrName, string claimPath);

    }
}
