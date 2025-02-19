using InterviewProjectTemplate.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewProjectTemplate.Data.Seeds
{
    public static class DefaultAdmin
    {
        public static void Seed(UserManager<ApplicationUser> userManager)
        {
            var rolesToAdd = new Roles[]
            {
                Roles.Admin
            };

            //Seed Default User
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@mycompay.com.au",
                EmailConfirmed = true,
                
            };

            SeedUser.SeedUserInDb(userManager, adminUser, "Password123$", rolesToAdd);
        }


    }
}
