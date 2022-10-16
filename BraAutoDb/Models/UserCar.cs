using BraAutoDb.Dal;
using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "user_car")]
    public class UserCar : BaseModel<uint>
    {
        [Column(Name = "user_id")]
        public uint? UserId { get; set; }
        public User User { get; set; }

        [Column(Name = "car_id")]
        public uint CarId { get; set; }

        public Car Car { get; set; }

        [Column(Name = "user_car_type_id")]
        public uint UserCarTypeId { get; set; }

        [Column(Name = "date")]
        public DateTime? Date { get; set; }

        [Column(Name = "description")]
        public string Description { get; set; }

        [Column(Name = "creator_id")]
        public uint CreatorId { get; set; }

        public User Creator { get; set; }

        public void LoadUser()
        {
            this.User = Db.Users.GetById(this.UserId.Value);
        }
    }
}
