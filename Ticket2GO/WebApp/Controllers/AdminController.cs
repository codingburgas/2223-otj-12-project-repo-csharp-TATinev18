using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> ManageUsers()
        {
            return View(await _adminService.GetUsers());
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _adminService.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(await _adminService.GetUserInformation(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ManageUserRolesViewModel model)
        {
            var user = await _adminService.GetUser(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _adminService.EditUser(model, user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the user.");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                IdentityResult resetResult = await _adminService.ResetPassword(model, user);

                if (!resetResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the password.");
                    return View(model);
                }
            }

            await _adminService.UpdateUserRole(model, user);
            await _adminService.UpdateAdminTable(user);

            return RedirectToAction(nameof(ManageUsers));
        }

        public async Task<IActionResult> DisplayAdmins()
        {
            var admins = await _adminService.GetAdminUsers();
            return View(admins);
        }
    }

}