using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "car")]
    public class Year : BaseModel<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }
    }
}
