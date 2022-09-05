using DapperMySqlMapper;

namespace BraAutoDb.Models
{ 
    [Table(Name = "user_in_role")]
    public class UserInRole
    {
        [Column(Name = "user_id")]
        public uint UserId { get; set; }

        [Column(Name = "user_role_id")]
        public uint UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
    }
}
