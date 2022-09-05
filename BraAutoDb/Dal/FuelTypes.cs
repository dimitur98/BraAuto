using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class FuelTypes
    {
        public static List<FuelType> GetAll()
        {
            var sql = @"
                SELECT *
                FROM fuel_type
                ORDER BY sort_order";

            return Db.Mapper.Query<FuelType>(sql).ToList();
        }
    }
}
