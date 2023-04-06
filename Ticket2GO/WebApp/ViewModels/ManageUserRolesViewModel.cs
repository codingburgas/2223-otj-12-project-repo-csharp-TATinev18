namespace WebApp.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public IList<string> Roles { get; set; }
        public IList<string> AvailableRoles { get; set; }
    }
}
