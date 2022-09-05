using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "gearbox_type")]
    public class GearboxType : BaseModel<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }
    }
}
