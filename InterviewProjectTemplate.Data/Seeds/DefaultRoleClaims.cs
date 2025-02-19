using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Seeds
{
    internal static class DefaultRoleClaims
    {
        internal static void ApplyRoleClaims(ApplicationRole identityRole,
           RoleManager<ApplicationRole> roleManager)
        {
            var roleType = (Roles)Enum.Parse<Roles>(identityRole.Name);
            var roleClaims = roleManager.GetClaimsAsync(identityRole).Result;

            var newClaims = new List<Claim>();

            void AddClaimPermission(params string[] claims)
            {
                foreach (string claim in claims)
                {
                    if (!newClaims.Exists(x => x.Value == claim))
                    {
                        newClaims.Add(new Claim(ClaimType.Permissions, claim));
                    }
                }
            }

            switch (roleType)
            {
                case Roles.Admin:
                    AddClaimPermission("admin",
                        ClaimPermission.Read,   // This is deliberate - to check dupes
                        ClaimPermission.Create,
                        ClaimPermission.Update,
                        ClaimPermission.Delete);
                    break;
                default:
                    AddClaimPermission(ClaimPermission.Read);
                    break;
            }

            var currentPermissionClaims = roleClaims.Where(x => x.Type == ClaimType.Permissions);

            // now add new claims without existing
            newClaims = newClaims.Where(x => !currentPermissionClaims
                    .Any(rc => rc.Value == x.Value))
                .ToList();

            foreach (Claim newClaim in newClaims)
            {
                //newClaim.

                // now add newclaims to role
                var result = roleManager.AddClaimAsync(identityRole, newClaim).Result;
                if (!result.Succeeded)
                {
                    var errors = JoinErrorString(result.Errors);
                    throw new Exception($"Failed to add claim {newClaim.Value} - ERRORS:\n {errors}");
                }
            }
        }

        private static string JoinErrorString(IEnumerable<IdentityError> errors)
        {
            return string.Join(Environment.NewLine,
                errors.Select(x => $"{x.Code}={x.Description}"));
        }
    }
}
