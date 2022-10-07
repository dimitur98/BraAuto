using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class UserCarTypes : BaseDal<UserCarType>
    {
        public readonly uint FavouriteId = 1;
        public readonly uint CompareId = 2;
        public readonly uint ServiceAppointmentId = 3;
        public readonly uint ServiceAppointmentApprovedId = 4;
        public readonly uint InspectionId = 5;
        public readonly uint RepairingId = 6;
        public readonly uint FinishedId = 7;

        public UserCarTypes() : base("user_car_type", "id", "id") { }
    }
}
