using BraAutoDb.Models;
using BraAutoDb.Models.UsersSearch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BraAuto.ViewModels
{
    public abstract class UserSearchBaseModel : BaseSearchModel<Response, User>
    {
        public UserSearchBaseModel()
        {
            this.SortFields = new List<(string Name, string SortColumn, bool SortDesc, bool Specific)> { ("Newest First", "u.created_at", true, false), ("Oldest First", "u.created_at", false, false), ("Active First", "u.is_active", true, true), ("Not Active First", "u.is_active", false, true) };
        }

        public virtual Request ToSearchRequest()
        {
            var request = new Request();

            this.SetSearchRequest(request);

            return request;
        }
    }

    public class UserAdminSearchModel : UserSearchBaseModel
    {
        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("My", "Cars"), ("Admin", "Users") };

            return new Breadcrumb(paths);
        }
    }

    public class UserServiceSearchModel : UserSearchBaseModel
    {
        public string Keywords { get; set; }

        public uint UserTypeId { get; set; }

        public uint? LocationId { get; set; }
        public IEnumerable<Location> Locations { get; set; }

        public override Request ToSearchRequest()
        {
            var request = new Request()
            {
                Keywords = this.Keywords,
                UserTypeId = this.UserTypeId,
                LocationId = this.LocationId
            };

            this.SetSearchRequest(request);

            return request;
        }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("ServiceSearch", "Users") };

            return new Breadcrumb(paths);
        }
    }

    public class UserBaseModel
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Birthday")]
        public DateTime? Birthday { get; set; }

        [Phone]
        [DisplayName("Mobile")]
        public string? Mobile { get; set; }

        [DisplayName("Description")]
        public string? Description { get; set; }

        [DisplayName("Location")]
        public uint? LocationId { get; set; }
        public IEnumerable<Location> Locations { get; set; }

        [DisplayName("Specific Location")]
        public string SpecificLocation { get; set; }

        [DisplayName("User Type")]
        public uint UserTypeId { get; set; }

        public IFormFile Photo { get; set; }

        [DisplayName("Booking Interval Hours")]
        public uint? BookingIntervalHours { get; set; }

        public uint? EndWorkingTime { get; set; }

        [DisplayName("Working Time From (0-23h)")]
        public uint? StartWorkingTime { get; set; }

        [DisplayName("Max Bookings Per Day")]
        public uint? MaxBookingsPerDay { get; set; }

        public IEnumerable<UserType>? UserTypes { get; set; }

        public bool IsAdminUserEditPage { get; set; }
    }

    public class UserRegisterModel : UserBaseModel
    {
        [Required]
        [DisplayName("Username")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "The {0} cannot contain empty spaces.")]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Register", "Users") };

            return new Breadcrumb(paths);
        }
    }

    public class UserEditModel : UserBaseModel
    {
        [Key]
        [Required]
        [HiddenInput(DisplayValue = false)]
        public uint Id { get; set; }

        [ReadOnly(true)]
        [DisplayName("Username")]
        public string Username { get; set; }

        [ReadOnly(true)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        public string PhotoUrl { get; set; }

        [DisplayName("Roles")]
        public IEnumerable<uint> UserRoleIds { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), this.IsAdminUserEditPage ? ("Admin", "Users") : ("My", "Cars"), ("Edit", "Users") };

            return new Breadcrumb(paths);
        }
    }

    public class LoginUserModel
    {
        [Required]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Login", "Users") };

            return new Breadcrumb(paths);
        }
    }

    public class ChangePasswordUserModel
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("My", "Cars"), ("ChangePassword", "Users") };

            return new Breadcrumb(paths);
        }
    }

    public class UserSetUsernameModel
    {

        [Required]
        [DisplayName("Username")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "The {0} cannot contain empty spaces.")]
        public string Username { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Login", "Users"), ("SetUsername", "Users") };

            return new Breadcrumb(paths);
        }
    }

    public class UserServiceDetailsModel
    {
        public uint Id { get; set; }

        [DisplayName("Owner Name")]
        public string Name { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Mobile")]
        public string Mobile { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        public string SpecificLocation { get; set; }

        public string PhotoUrl { get; set; }
    }
}
