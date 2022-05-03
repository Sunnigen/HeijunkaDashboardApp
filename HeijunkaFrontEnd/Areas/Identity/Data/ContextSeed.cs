﻿using Microsoft.AspNetCore.Identity;

/*
 ContextSeed.cs is used to define user roles.

 If database does not contain roles, it will automatically seed them in Program.cs

 Also defines some users for production/testing use.
*/

namespace HeijunkaFrontEnd.Areas.Identity.Data
{
    public class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<HeijunkaUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(UserRoleEnums.SuperUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoleEnums.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoleEnums.Supervisor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoleEnums.ProductionControl.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoleEnums.Assembler.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoleEnums.Observer.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<HeijunkaUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new HeijunkaUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                //FirstName = "k",
                //LastName = "a",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "super");
                    //await userManager.AddToRoleAsync(defaultUser, UserRoleEnums.Observer.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, UserRoleEnums.Assembler.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, UserRoleEnums.ProductionControl.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, UserRoleEnums.Supervisor.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, UserRoleEnums.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, UserRoleEnums.SuperUser.ToString());
                }

            }
        }

    }
}
