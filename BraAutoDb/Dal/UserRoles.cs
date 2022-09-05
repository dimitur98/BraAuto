using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class UserRoles
    {
        public static List<UserRole> GetUserRoles(IEnumerable<uint> ids)
        {
            string sql = @"
                SELECT *
                FROM user_role
                WHERE id IN @ids
                ORDER BY `order`";

            return Db.Mapper.Query<UserRole>(sql, new { ids }).ToList();
        }
    }
}
