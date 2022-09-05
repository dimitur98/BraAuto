using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class Stages
    {
        public static List<Stage> GetAll()
        {
            var sql = "SELECT * FROM stage";

            return Db.Mapper.Query<Stage>(sql).ToList();
        }
    }
}
