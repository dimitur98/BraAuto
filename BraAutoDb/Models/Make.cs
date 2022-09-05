using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "make")]
    public class Make : BaseModel<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }
    }
}
