using BraAutoDb.Dal;
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
        public Make Make { get; set; }

        [Column(Name = "vehicle_type_id")]
        public uint VehicleTypeId { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }

        public void LoadMake()
        {
            this.Make = Db.Makes.GetById(this.MakeId);
        }
    }
}
