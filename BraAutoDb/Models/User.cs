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
        public string Descripton { get; set; }

        [Column(Name = "location")]
        public string Location { get; set; }

        [Column(Name = "is_active")]
        public bool IsActive { get; set; }

        [Column(Name = "user_type_id")]
        public uint UserTypeId { get; set; }

        public virtual IEnumerable<UserInRole> Roles { get; set; }

        public void LoadUserInRoles() => this.Roles = Db.UserInRoles.GetByUserId(this.Id);
    }
}
