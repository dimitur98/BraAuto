using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "car_view")]
    public class CarView : BaseModel<uint>
    {
        [Column(Name = "car_id")]
        public uint CarId { get; set; }

        [Column(Name = "user_ip")]
        public string UserIp { get; set; }

        [Column(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
