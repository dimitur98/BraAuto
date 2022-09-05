using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class Years
    {
        public static List<Year> GetAll()
        {
            var sql = @"
                SELECT *
                FROM year
                ORDER BY sort_order DESC";

            return Db.Mapper.Query<Year>(sql).ToList();
        }
    }
}
