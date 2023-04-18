using Microsoft.AspNetCore.Identity;
using WebApp.Areas.Identity.Data;
using WebApp.ViewModels;

namespace WebApp.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<List<ManageUserRolesViewModel>> GetUsers();
        public Task<ApplicationUser> GetUser(string userId);
        public Task<ManageUserRolesViewModel> GetUserInformation(ApplicationUser user);
        public Task<IdentityResult> EditUser(ManageUserRolesViewModel model, ApplicationUser user);
        public Task<IdentityResult> ResetPassword(ManageUserRolesViewModel model, ApplicationUser user);
        public Task UpdateUserRole(ManageUserRolesViewModel model, ApplicationUser user);
        public Task UpdateAdminTable(ApplicationUser user);
        public Task<List<ApplicationUser>> GetAdminUsers();
    }
}
