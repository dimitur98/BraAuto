using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class UserRoles : BaseDal<UserRole>
    {
        public readonly uint UserId = 2;

        public UserRoles() : base("user_role", "id", "sort_order") { }
    }
}
