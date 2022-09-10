using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class UserInRoles : BaseDal<UserInRole>
    {
        public UserInRoles() : base("user_in_role", "id", "user_id") { }

        public void Insert(UserInRole userInRole)
        {
            var sql = @"INSERT INTO `user_in_role`
                        (
                            `user_id`,
                            `user_role_id`
                        )VALUES(
                            @userId,
                            @userRoleId
                        );

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                userId = userInRole.UserId,
                userRoleId  = userInRole.UserRoleId
            };

            userInRole.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public IEnumerable<UserInRole> GetByUserId(uint userId) => this.GetByFieldValues("user_id", new uint[] { userId });

        public void DeleteByUserId(uint userId) => this.DeleteByField(new uint[] { userId }, "user_id");

        public void LoadUserRoles(IEnumerable<UserInRole> userInRoles)
        {
            Db.LoadEntities(userInRoles, uir => uir.UserRoleId, userRoleIds => Db.UserRoles.GetByIds(userRoleIds), (userInRole, userRoles) => userInRole.UserRole = userRoles.FirstOrDefault(ur => ur.Id == userInRole.UserRoleId));
        }
    }
}
