using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "color")]
    public class Color : BaseModel<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }
    }
}
