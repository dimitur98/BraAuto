using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class GearboxTypes
    {
        public static List<GearboxType> GetAll()
        {
            var sql = @"
                SELECT *
                FROM gearbox_type
                ORDER BY sort_order";

            return Db.Mapper.Query<GearboxType>(sql).ToList();
        }
    }
}
