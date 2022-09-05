using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "model")]
    public class Model : BaseModel<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "make_id")]
        public uint MakeId { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }
    }
}
