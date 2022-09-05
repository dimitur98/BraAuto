using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class Models
    {
        public static List<Model> GetByMakeId(uint makeId)
        {
            var sql = @"
                SELECT * 
                FROM model
                WHERE make_id = @makeId";

            return Db.Mapper.Query<Model>(sql, new { makeId }).ToList();
        }
    }
}
