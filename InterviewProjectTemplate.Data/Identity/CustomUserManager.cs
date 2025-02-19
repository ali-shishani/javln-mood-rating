using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    public class CustomUserManager : UserManager<ApplicationUser>
    {
        private readonly IHostEnvironment _environment;

        public CustomUserManager(
            IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<ApplicationUser>> logger,
            IHostEnvironment environment)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _environment = environment;
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {

            // Always consider the email confirmed for now
            user.EmailConfirmed = true;

            // Use the default behavior in other environments
            return await base.CreateAsync(user);
        }
    }
}
