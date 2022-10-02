using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "user_car_type")]
    public class UserCarType : BaseModel<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }
    }
}
