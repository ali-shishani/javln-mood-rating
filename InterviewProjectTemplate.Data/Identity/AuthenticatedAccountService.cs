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
    public class AuthenticatedAccountService : IAuthenticatedAccountService
    {
        private readonly IAuthenticatedUserProvider _authUserProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MoodRatingDbContext _db;
        private readonly ILogger<AuthenticatedAccountService> _logger;

        public AuthenticatedAccountService(ILogger<AuthenticatedAccountService> logger,
                                            IAuthenticatedUserProvider authenticatedUserProvider,
                                            UserManager<ApplicationUser> userManager,
                                            MoodRatingDbContext db)
        {
            _authUserProvider = authenticatedUserProvider;
            _userManager = userManager;
            _db = db;
            _logger = logger;
        }

        private AuthenticatedUserModel GetCurrentUserModel()
        {
            var user = _authUserProvider.GetCurrentUser();
            if (user == null)
            {
                throw new AccountException(AccountErrorCode.LOGIN_USER_NOT_AUTHENTICATED, "No authenticated user",
                    (int)HttpStatusCode.Unauthorized);
            }
            return user;
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            var userModel = GetCurrentUserModel();
            return await _userManager.FindByIdAsync(userModel.UserId.ToString());
        }

        public async Task<UserProfileResponse> GetAccountUserProfile()
        {
            var user = await GetCurrentUser();
            var resp = new UserProfileResponse()
            {
                Id = user.Id,
                //FirstName = user.FirstName,
                //LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                //MiddleName = user.MiddleName
            };

            var profile = await _db.UserDetails
                .AsNoTracking()
                .Select(x => new
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    DateOfBirth = x.Dob
                    //Addresses = (IEnumerable<AddressModel>)x.UserAddresses
                    //    .Where(a => a.IsDeleted == false)
                    //    .ToList()
                })
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (profile != null)
            {
                resp.DateOfBirth = profile.DateOfBirth;
                //resp.Addresses = profile.Addresses;
                resp.HasUserProfile = true;
            }

            return resp;
        }

        //public async Task<UserProfileResponse> CreateUserProfile(UserProfileCreateRequest request)
        //{
        //    var user = await GetCurrentUser();
        //    var userId = user.Id;
        //    if (await _db.UserDetails.AnyAsync(x => x.UserId == userId))
        //    {
        //        throw new AccountException(AccountErrorCode.LOGIN_USER_PROFILE_EXISTS, "User profile already exists");
        //    }

        //    var addr = request.Addresses.Select(x => new UserAddress(userId, x)
        //    {
        //        // CreatedBy is the current user as its for the account
        //        CreatedBy = userId,
        //        CreatedUtc = DateTime.UtcNow,
        //    }).ToList();

        //    foreach (UserAddress userAddress in addr)
        //    {
        //        if (userAddress.NormalizedAddress == null)
        //        {
        //            userAddress.NormalizeAddress();
        //        }
        //    }

        //    var userProfile = new UserDetail()
        //    {
        //        // record
        //        CreatedDateUtc = DateTime.UtcNow,

        //        // profile
        //        Dob = request.DateOfBirth,
        //        UserAddresses = addr,

        //        // user
        //        UserId = user.Id
        //    };

        //    await _db.UserDetails.AddAsync(userProfile);
        //    await _db.SaveChangesAsync();
        //    _logger.LogInformation("UserProfile Created: {Email} [{Id}] UserDetailsId:{Id}",
        //            user.Email, user.Id, userProfile.Id);

        //    return Map(userProfile, user);
        //}

        private UserProfileResponse Map(UserDetail userProfile, ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(userProfile, nameof(userProfile));
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            return new UserProfileResponse()
            {
                Id = userProfile.Id,
                UserId = userProfile.UserId,
                Email = user.Email,
                //FirstName = user.FirstName,
                //MiddleName = user.MiddleName,
                //LastName = user.LastName,
                HasUserProfile = true,
                DateOfBirth = userProfile.Dob,
                Phone = user.PhoneNumber,
                //Addresses = (IEnumerable<AddressModel>)userProfile.UserAddresses
            };
        }

        //public async Task<UserProfileResponse> UpdateUserProfileDetails(UserProfileUpdateRequest req)
        //{
        //    var user = await GetCurrentUser();
        //    var profile = await _db.UserDetails
        //                           .FirstOrDefaultAsync(x => x.UserId == user.Id);

        //    if (profile == null)
        //    {
        //        throw new AccountException(AccountErrorCode.LOGIN_USER_PROFILE_NOTFOUND,
        //            $"User profile not found for user {user.Id}", (int)HttpStatusCode.NotFound);
        //    }

        //    // lets update user if need be
        //    bool isUserDirty = false;
        //    bool isUserProfileDirty = false;
        //    if (!string.IsNullOrEmpty(req.FirstName) && user.FirstName != req.FirstName)
        //    {
        //        user.FirstName = req.FirstName;
        //        isUserDirty = true;
        //    }
        //    if (!string.IsNullOrEmpty(req.MiddleName) && user.MiddleName != req.MiddleName)
        //    {
        //        user.MiddleName = req.MiddleName;
        //        isUserDirty = true;
        //    }
        //    if (!string.IsNullOrEmpty(req.LastName) && user.LastName != req.LastName)
        //    {
        //        user.LastName = req.LastName;
        //        isUserDirty = true;
        //    }

        //    if (isUserDirty)
        //    {
        //        await _userManager.UpdateAsync(user);
        //    }

        //    if (req.DateOfBirth.HasValue)
        //    {
        //        profile.Dob = req.DateOfBirth.Value;
        //        isUserProfileDirty = true;
        //    }

        //    if (req.Addresses != null && req.Addresses.Any())
        //    {
        //        // updating address history, mark old ones SOFT DELETE
        //        var addrIds = req.Addresses.Select(x => x.Id);

        //        var userAddresses = await _db.UserAddresses
        //            .Where(x => x.UserId == user.Id
        //                        && x.IsDeleted == false
        //            )
        //            .ToListAsync();

        //        // now mark current ones as deleted (currently user has ONE active address) - yet we want to keep for history
        //        // TODO: 2024-03-08 in future, we may want to allow user to have MULTIPLE addresses
        //        // in which case we simply fillter
        //        // like :
        //        // var userAddressesToDelete = userAddresses.Where(x=>addrIds.Contains(x.Id)).ToList()    // .ToList() important
        //        foreach (var currentAddress in userAddresses)
        //        {
        //            currentAddress.DeletedUtc = DateTime.UtcNow;
        //            currentAddress.DeletedBy = user.Id;
        //            _db.Entry(currentAddress).State = EntityState.Modified;
        //        }

        //        // lets add new ones
        //        var replacementAddresses = req.Addresses.Select(x => new UserAddress(x)
        //        {
        //            CreatedBy = user.Id,
        //            CreatedUtc = DateTime.UtcNow,
        //            UserId = user.Id,
        //            UserDetailId = profile.Id
        //        }).ToList();
        //        foreach (UserAddress replacementAddress in replacementAddresses)
        //        {
        //            if (replacementAddress.NormalizedAddress == null)
        //            {
        //                replacementAddress.NormalizeAddress();
        //            }
        //        }

        //        await _db.UserAddresses.AddRangeAsync(replacementAddresses);
        //        _logger.LogDebug("UserProfile: {Email} - Add {Count} address(es)",
        //            user.Email, replacementAddresses.Count);
        //    }

        //    if (isUserProfileDirty)
        //    {
        //        profile.ModifiedDateUtc = DateTime.UtcNow;
        //    }
        //    await _db.SaveChangesAsync();
        //    _logger.LogInformation("UserProfile:{Email} updated", user.Email);

        //    return await GetAccountUserProfile();
        //}
    }
}
