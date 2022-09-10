using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "condition")]
    public class Condition : BaseModel<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }
    }
}
