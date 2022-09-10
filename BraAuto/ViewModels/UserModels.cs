using BraAuto.ViewModels;
using BraAutoDb.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BraAuto.ViewModels
{
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
        public string? Location { get; set; }

        [DisplayName("User Type")]
        public uint UserTypeId { get; set; }

        public IEnumerable<UserType>? UserTypes { get; set; }
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


        [DisplayName("Roles")]
        public IEnumerable<uint> UserRoleIds { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("My", "Cars"), ("Edit", "Users") };

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
}
