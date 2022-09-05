using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "car_in_car_service")]
    public class CarInCarService : BaseModel<uint>
    {
        [Column(Name = "car_id")]
        public uint CarId { get; set; }

        [Column(Name = "car_service_id")]
        public uint CarServiceId { get; set; }

        [Column(Name = "stage_id")]
        public uint StageId { get; set; }
    }
}
