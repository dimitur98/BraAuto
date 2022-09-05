using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "user_role")]
    public class UserRole : BaseModel<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "description")]
        public string Description { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }
    }
}
