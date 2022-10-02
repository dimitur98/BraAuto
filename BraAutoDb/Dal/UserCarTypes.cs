using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class UserCarTypes : BaseDal<UserCarType>
    {
        public readonly uint FavouriteId = 1;
        public readonly uint CompareId = 2;

        public UserCarTypes() : base("user_car_type", "id", "id") { }
    }
}
