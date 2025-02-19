using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Seeds
{
    internal static class SeedUser
    {
        public static async Task<Guid?> SeedUserInDb(UserManager<ApplicationUser> userManager,
            ApplicationUser defaultUser,
            IEnumerable<Roles>? rolesToAdd)
        {
            var usr = await userManager.FindByEmailAsync(defaultUser.Email);
            if (usr != null)
            {
                return usr.Id;
            }

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    var identityResult = await userManager.CreateAsync(defaultUser);
                    if (!identityResult.Succeeded)
                    {
                        throw new Exception($"unable to create user {defaultUser.Email} in db");
                    }

                    if (rolesToAdd != null && rolesToAdd.Any())
                    {
                        foreach (Roles roles in rolesToAdd)
                        {
                            await userManager.AddToRoleAsync(defaultUser, roles.ToString());
                        }
                    }


                    return defaultUser.Id;
                }
            }
            return null;
        }


        public static Guid? SeedUserInDb(UserManager<ApplicationUser> userManager,
            ApplicationUser defaultUser,
            string? password,
            IEnumerable<Roles>? rolesToAdd)
        {
            var usr = userManager.FindByEmailAsync(defaultUser.Email).Result;
            if (usr != null)
            {
                return usr.Id;
            }

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = userManager.FindByEmailAsync(defaultUser.Email).Result;
                if (user == null)
                {
                    var identityResult = userManager.CreateAsync(defaultUser, password).Result;
                    if (!identityResult.Succeeded)
                    {
                        throw new Exception($"unable to create user {defaultUser.Email} in db");
                    }

                    if (rolesToAdd != null && rolesToAdd.Any())
                    {
                        foreach (Roles roles in rolesToAdd)
                        {
                            userManager.AddToRoleAsync(defaultUser, roles.ToString());
                        }
                    }


                    return defaultUser.Id;
                }
            }
            return null;
        }
    }
}
