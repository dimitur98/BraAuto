using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static  class BodyTypes
    {
        public static List<BodyType> GetAll()
        {
            var sql = @"
                SELECT *
                FROM body_type
                ORDER BY sort_order";

            return Db.Mapper.Query<BodyType>(sql).ToList();
        }
    }
}
