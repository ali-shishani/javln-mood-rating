using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Seeds
{
    public static class DefaultRoles
    {
        public static void Seed(RoleManager<ApplicationRole> roleManager)
        {
            var roles = Enum.GetValues(typeof(Roles))
                .Cast<Roles>()
                .Select(x => x.ToString())
                .ToList();

            foreach (string role in roles)
            {
                var identityRole = roleManager.FindByNameAsync(role).Result;
                if (identityRole == null)
                {
                    identityRole = new ApplicationRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = role,
                    };

                    var result = roleManager.CreateAsync(identityRole).Result;

                    if (!result.Succeeded)
                    {
                        var errorString = JoinErrorString(result.Errors);
                        Console.WriteLine($"Failed to add new role {role} - ERRORS:{Environment.NewLine}{errorString}");
                        throw new Exception(errorString);
                    }
                }
                DefaultRoleClaims.ApplyRoleClaims(identityRole, roleManager);
            }
        }

        private static string JoinErrorString(IEnumerable<IdentityError> errors)
        {
            return string.Join(Environment.NewLine,
                errors.Select(x => $"{x.Code}={x.Description}"));
        }
    }
}
