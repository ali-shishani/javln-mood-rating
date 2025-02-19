using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Identity
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MoodRatingDbContext _db;
        private readonly MoodRatingDbContext _identityDbContext;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<UserAccountService> _logger;


        public UserAccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            MoodRatingDbContext db, 
            ILogger<UserAccountService> logger)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
            _identityDbContext = db;
            _roleManager = roleManager;
        }

        private async Task<ApplicationUser> FindRequiredUserByUserName(string userName)
        {
            var findUser = await _userManager.FindByNameAsync(userName);

            if (findUser == null)
            {
                _logger.LogWarning($"User {userName} is not found");
                throw new AccountException(AccountErrorCode.LOGIN_USER_NOT_FOUND, $"user not found: {userName}");
            }

            return findUser;
        }

        private async Task<ApplicationUser> FindRequiredUserByUserId(string userId)
        {
            var findUser = await _userManager.FindByIdAsync(userId);

            if (findUser == null)
            {
                _logger.LogWarning($"User {userId} is not found");
                throw new AccountException(AccountErrorCode.LOGIN_USER_NOT_FOUND, $"user not found: {userId}");
            }

            return findUser;
        }

        
        public async Task<IEnumerable<RoleInfo>> GetUserRoles(Guid userId)
        {
            var user = await FindRequiredUserByUserId(userId.ToString());

            // TODO: 2024-03-02 this NEEDS caching
            var roles = await _identityDbContext.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => new RoleInfo()
                {
                    RoleId = x.RoleId,
                    RoleName = x.Role.Name
                })
                .ToListAsync();

            return roles;
        }
    }
}
