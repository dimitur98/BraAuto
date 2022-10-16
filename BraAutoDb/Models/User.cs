using BraAutoDb.Dal;
using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "user")]
    public class User : AuditInfo<uint>
    {
        [Column(Name = "username")]
        public string Username { get; set; }

        [Column(Name = "password")]
        public string Password { get; set; }

        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "email")]
        public string Email { get; set; }

        [Column(Name = "birthday")]
        public DateTime? Birthday { get; set; }

        [Column(Name = "mobile")]
        public string Mobile { get; set; }

        [Column(Name = "description")]
        public string Description { get; set; }

        [Column(Name = "location_id")]
        public uint? LocationId { get; set; }

        [Column(Name = "specific_location")]
        public string SpecificLocation { get; set; }

        [Column(Name = "is_active")]
        public bool IsActive { get; set; }

        [Column(Name = "user_type_id")]
        public uint UserTypeId { get; set; }

        [Column(Name = "photo_url")]
        public string PhotoUrl { get; set; }

        [Column(Name = "booking_interval_hours")]
        public uint? BookingIntervalHours { get; set; }

        [Column(Name = "start_working_time")]
        public uint? StartWorkingTime { get; set; }

        [Column(Name = "end_working_time")]
        public uint? EndWorkingTime { get; set; }

        [Column(Name = "max_bookings_per_day")]
        public uint? MaxBookingsPerDay { get; set; }

        [Column(Name = "is_password_required")]
        public bool IsPasswordRequired { get; set; }

        public virtual IEnumerable<UserInRole> Roles { get; set; }

        public void LoadUserInRoles() => this.Roles = Db.UserInRoles.GetByUserId(this.Id);
    }
}
