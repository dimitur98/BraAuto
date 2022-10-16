using BraAutoDb.Dal;
using BraAutoDb.Models;
using BraAutoDb.Models.UserCarsSearch;

namespace BraAuto.Helpers.Extensions
{
    public static class UserExtensions
    {
        public static bool IsAdmin(this User user) => user.IsInRole("administrator");

        public static bool IsBlogger(this User user) => user.IsInRole("blogger");

        public static bool IsService(this User user) => user.IsOfType(Db.UserTypes.ServiceId);

        public static bool IsInRole(this User user, string role)
        {
            if (user == null) { return false; }

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
    
        public static bool IsOfType(this User user, uint userTypeId)
        {
            return user.UserTypeId == userTypeId;
        }

        public static List<uint> GetFreeHours(this User user, DateTime date)
        {
            var hours = new List<uint>();

            if (user.UserTypeId != Db.UserTypes.ServiceId) { return hours; }

            var serviceCars = Db.UserCars.Search(new Request { UserCarTypeIds = new uint[] { Db.UserCarTypes.ServiceAppointmentId, Db.UserCarTypes.ServiceAppointmentApprovedId }, UserId = user.Id, Date = date }).Records;

            for (uint i = user.StartWorkingTime.Value; i < user.EndWorkingTime.Value; i += user.BookingIntervalHours.Value)
            {
                hours.Add(i);
            }

            if (!serviceCars.IsNullOrEmpty())
            {
                foreach (var serviceCar in serviceCars)
                {
                    hours.Remove((uint)serviceCar.Date.Value.Hour);
                }
            }

            return hours;
        }
    }
}
