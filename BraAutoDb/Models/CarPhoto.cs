using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "car_photo")]
    public class CarPhoto : BaseModel<uint>
    {
        [Column(Name = "url")]
        public string Url { get; set; }

        [Column(Name = "car_id")]
        public uint CarId { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }
    }
}
