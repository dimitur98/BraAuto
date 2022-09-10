using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class UserTypes : BaseDal<UserType>
    {
        public readonly uint UserId = 1;
        public readonly uint ServiceId = 2;

        public UserTypes() : base("user_type", "id", "sort_order") { }
    }
}
