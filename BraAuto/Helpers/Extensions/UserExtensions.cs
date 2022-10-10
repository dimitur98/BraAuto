using BraAutoDb.Dal;
using BraAutoDb.Models;
using System.Runtime.CompilerServices;

namespace BraAuto.Helpers.Extensions
{
    public static class UserExtensions
    {
        public static bool IsAdmin(this User user) => user.IsInRole("administrator");

        public static bool IsBlogger(this User user) => user.IsInRole("blogger");

        public static bool IsService(this User user) => user.IsOfType(Db.UserTypes.ServiceId);

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
    
        public static bool IsOfType(this User user, uint userTypeId)
        {
            return user.UserTypeId == userTypeId;
        }

        public static List<uint> GetFreeHours(this User user, DateTime date)
        {
            var hours = new List<uint>();

            if (user.UserTypeId != Db.UserTypes.ServiceId) { return hours; }

            var serviceCars = Db.UserCars.Get(new uint[] { Db.UserCarTypes.ServiceAppointmentId, Db.UserCarTypes.ServiceAppointmentApprovedId }, userId: user.Id, date: date);

            for (uint i = user.StartWorkingTime; i < user.EndWorkingTime; i += user.BookingIntervalHours)
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
