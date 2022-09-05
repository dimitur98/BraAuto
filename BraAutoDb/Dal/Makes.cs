using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class Makes
    {
        public static List<Make> GetAll()
        {
            var sql = @"
                SELECT *
                FROM make
                ORDER BY sort_order";

            return Db.Mapper.Query<Make>(sql).ToList();
        }
    }
}
