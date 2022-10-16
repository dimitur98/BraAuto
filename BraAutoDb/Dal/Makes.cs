using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Makes : BaseDal<Make>
    {
        public Makes() : base("make", "id", "sort_order") { }

        public List<Make> GetAll(uint vehicleTypeId)
        {
            var sql = @"
                SELECT * 
                FROM make m
                WHERE EXISTS(SELECT * FROM make_in_vehicle_type mivt WHERE m.id = mivt.make_id AND vehicle_type_id = @vehicleTypeId)";

            return Db.Mapper.Query<Make>(sql, new { vehicleTypeId }).ToList();
        }
    }
}