using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "user_car")]
    public class UserCar : BaseModel<uint>
    {
        [Column(Name = "user_id")]
        public uint UserId { get; set; }

        [Column(Name = "car_id")]
        public uint CarId { get; set; }

        [Column(Name = "user_car_type_id")]
        public uint UserCarTypeId { get; set; }
    }
}
