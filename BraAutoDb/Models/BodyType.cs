using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "body_type")]
    public class BodyType : BaseModel<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }

        public uint VehicleTypeId { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }
    }
}
