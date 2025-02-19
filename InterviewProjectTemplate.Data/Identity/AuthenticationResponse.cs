using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //TODO: 2024-04-22 The phone number is included to use the verify-phone endpoint upon user authentication. Consider removing this when the mobile verification process flow is changed.
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
        public List<RoleInfo> RolesInfo { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string JwToken { get; set; }
        public string RefreshToken { get; set; }
        public bool HasAuthenticator { get; set; }
        public bool TwoFactorClientRemembered { get; set; }
    }
}
