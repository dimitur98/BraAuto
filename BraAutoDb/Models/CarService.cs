using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "car_service")]
    public class CarService : AuditInfo<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "description")]
        public string Description { get; set; }

        [Column(Name = "img")]
        public string Img { get; set; }

        [Column(Name = "location")]
        public string Location { get; set; }
    }
}
