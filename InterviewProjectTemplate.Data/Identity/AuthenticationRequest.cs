using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// 2FA Code (ie Authenticator or SMS)
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Remember this device
        /// </summary>
        public bool RememberMe { get; set; }

    }
}
