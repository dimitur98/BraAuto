using BraAutoDb.Dal;
using BraAutoDb.Models;

namespace BraAuto.Helpers.Extensions
{
    public static class UserExtensions
    {
        public static bool IsAdmin(this User user) => user.IsInRole("administrator");

        public static bool IsBlogger(this User user) => user.IsInRole("blogger");

        public static bool IsInRole(this User user, string role)
        {
            var principal = new HttpContextAccessor()?.HttpContext?.User;

            if (principal?.Identity?.Name == user.Username && principal.IsInRole(role) == true) { return true; }

            if (user.Roles == null)
            {
                user.LoadUserInRoles();
            }

            if (user.Roles.Any(ur => ur.UserRole == null))
            {
                Db.UserInRoles.LoadUserRoles(user.Roles);
            }

            if (user.Roles != null && user.Roles.Any(uir => uir.UserRole.Name.Replace(" ", "-").ToLower() == role)) { return true; }

            return false;
        }
    }
}
