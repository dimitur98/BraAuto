using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class UserInRoles
    {
        public static List<UserInRole> GetByUserId(uint userId) => UserInRoles.GetByUsers(new uint[] { userId });

        public static List<UserInRole> GetByUsers(IEnumerable<uint> userIds)
        {
            string sql = @"
                SELECT *
                FROM user_in_role               
                WHERE user_id IN @userIds";

            return Db.Mapper.Query<UserInRole>(sql, new { userIds }).ToList();
        }

        public static void LoadUserRoles(IEnumerable<UserInRole> userInRoles)
        {
            Db.LoadEntities(userInRoles, uir => uir.UserRoleId, userRoleIds => UserRoles.GetUserRoles(userRoleIds), (userInRole, userRoles) => userInRole.UserRole = userRoles.FirstOrDefault(ur => ur.Id == userInRole.UserRoleId));
        }
    }
}
