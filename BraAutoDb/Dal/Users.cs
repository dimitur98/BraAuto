using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Users : BaseDal<User>
    {
        public Users() : base("user", "id", "created_at") { }


        public User GetByUsername(string username) => this.GetByFieldValues<string>("username", new string[] { username }).FirstOrDefault();

        public User GetByEmail(string email) => this.GetByFieldValues<string>("email", new string[] { email }).FirstOrDefault();

        public User GetByUsernameAndPassword(string username, string password)
        {
            var sql = @"
                SELECT *
                FROM user
                WHERE username = @username AND password = fn_password(@password)";

            return Db.Mapper.Query<User>(sql, new { username, password }).FirstOrDefault();
        }

        public void Insert(User user)
        {
            var sql = @"INSERT INTO `user`
                        (
                            `username`,
                            `password`,
                            `name`,
                            `email`,
                            `birthday`,
                            `mobile`,
                            `description`,
                            `location`,
                            `is_active`,
                            `user_type_id`,
                            `created_at`,
                            `edited_at`
                        )VALUES(
                            @username,
                            fn_password(@password),
                            @name,
                            @email,
                            @birthday,
                            @mobile,
                            @description,
                            @location,
                            @isActive,
                            @userTypeId,
                            NOW(),
                            NOW()
                        );

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                username = user.Username,
                password = user.Password,
                name = user.Name,
                email = user.Email,
                birthday = user.Birthday,
                mobile = user.Mobile,
                description = user.Descripton,
                location = user.Location,
                isActive = user.IsActive,
                userTypeId = user.UserTypeId,
            };

            user.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public void Update(User user)
        {
            string sql = @"
                UPDATE `user`
                     SET name = @name,
                         email = @email,
                         birthday = @birthday,
                         mobile = @mobile,
                         description = @description,
                         location = @location,
                         is_active = @isActive,
                         user_type_id = @userTypeId,
                         editor_id = @editorId,
                         edited_at = NOW()
                WHERE id = @id";

            var queryParams = new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                birthday = user.Birthday,
                mobile = user.Mobile,
                description = user.Descripton,
                location = user.Location,
                isActive = user.IsActive,
                userTypeId = user.UserTypeId,
                editorId = user.EditorId
            };

            Db.Mapper.Execute(sql, queryParams);
        }

        public void SetPassword(uint id, string password)
        {
            string sql = @"
                UPDATE `user`
                    SET password = fn_password(@password)
                WHERE id = @id";

            Db.Mapper.Execute(sql, new { id, password });
        }
    }
}