using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "user")]
    public class User : BaseModel<uint>
    {
        [Column(Name = "username")]
        public string Username { get; set; }

        [Column(Name = "password")]
        public string Password { get; set; }

        [Column(Name = "first_name")]
        public string FirstName { get; set; }

        [Column(Name = "last_name")]
        public string LastName { get; set; }

        [Column(Name = "email")]
        public string Email { get; set; }

        [Column(Name = "birthday")]
        public DateTime Birthday { get; set; }

        [Column(Name = "mobile")]
        public string Mobile { get; set; }

        [Column(Name = "is_active")]
        public bool IsActive { get; set; }

        [Column(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [Column(Name = "editor_id")]
        public uint EditorId { get; set; }

        [Column(Name = "edited_at")]
        public DateTime EditedAt { get; set; }

        public virtual IEnumerable<UserInRole> Roles { get; set; }

        public void LoadUserInRoles() => this.Roles = Dal.UserInRoles.GetByUserId(this.Id);
    }
}
