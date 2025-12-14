using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.models;
using System;
using System.Threading.Tasks;

namespace SmartStockAI.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(ApplicationDBContext context, 
            UserManager<AppUser> userManager, 
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            
            await context.Database.MigrateAsync();

            await SeedRolesAsync(roleManager);
            
            await SeedAdminUserAsync(userManager);
            
            await SeedCategoriesAsync(context);
            
            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roleNames = { "Admin", "Staff" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@smartstock.ai") != null)
                return;

            var adminUser = new AppUser
            {
                UserName = "admin@smartstock.ai",
                Email = "admin@smartstock.ai",
                FullName = "Admin User",
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        private static async Task SeedCategoriesAsync(ApplicationDBContext context)
        {
            if (await context.Categories.AnyAsync(c => c.Name == "Uncategorized"))
                return;

            context.Categories.Add(new Category
            {
                Id = Guid.NewGuid(),
                Name = "Uncategorized",
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}