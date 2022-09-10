using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class UserRoles : BaseDal<UserRole>
    {
        public readonly uint UserRoleId = 2;

        public UserRoles() : base("user_role", "id", "sort_order") { }
    }
}
