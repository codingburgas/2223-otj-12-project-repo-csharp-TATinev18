using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Data;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public AdminService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<List<ManageUserRolesViewModel>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoleViewModels = new List<ManageUserRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var availableRoles = _roleManager.Roles.Select(r => r.Name).ToList();

                userRoleViewModels.Add(new ManageUserRolesViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles,
                    AvailableRoles = availableRoles
                });
            }

            return userRoleViewModels;
        }

        public async Task<ApplicationUser> GetUser(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }


        public async Task<ManageUserRolesViewModel> GetUserInformation(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var availableRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            return new ManageUserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles,
                AvailableRoles = availableRoles
            };
        }

        public async Task<IdentityResult> EditUser(ManageUserRolesViewModel model, ApplicationUser user)
        {
            if (!string.IsNullOrEmpty(model.Email))
            {
                user.Email = model.Email;
                user.UserName = model.Email;
            }

            if (!string.IsNullOrEmpty(model.FirstName))
            {
                user.FirstName = model.FirstName;
            }

            if (!string.IsNullOrEmpty(model.LastName))
            {
                user.LastName = model.LastName;
            }

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ResetPassword(ManageUserRolesViewModel model, ApplicationUser user)
        {
            IdentityResult resetResult = new IdentityResult();
            if (!string.IsNullOrEmpty(model.Password))
            {
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                resetResult = await _userManager.ResetPasswordAsync(user, passwordResetToken, model.Password);
            }

            return resetResult;
        }

        public async Task UpdateUserRole(ManageUserRolesViewModel model, ApplicationUser user)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.SelectedRoles.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(model.SelectedRoles);

            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            await UpdateAdminTable(user);
        }

        public async Task UpdateAdminTable(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var adminEntry = await _dbContext.Admins.FirstOrDefaultAsync(a => a.ApplicationUserId == user.Id);

            if (userRoles.Contains("Admin") && adminEntry == null)
            {
                var newAdmin = new Admin { ApplicationUserId = user.Id, ApplicationUser = user };
                await _dbContext.Admins.AddAsync(newAdmin);
            }
            else if (!userRoles.Contains("Admin") && adminEntry != null)
            {
                _dbContext.Admins.Remove(adminEntry);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ApplicationUser>> GetAdminUsers()
        {
            var admins = await _dbContext.Admins.Include(a => a.ApplicationUser).ToListAsync();
            return admins.Select(a => a.ApplicationUser).ToList();
        }

        public async Task UpdateAdminRoles(ApplicationUser user, IList<string> roles)
        {
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.AddToRolesAsync(user, roles);

            await UpdateAdminTable(user);
        }
    }
}
