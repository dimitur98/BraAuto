using BraAutoDb.Models;
using System.Data;

namespace BraAutoDb.Dal
{
    public static class Users
    {
        public static List<User> GetAll()
        {
            var sql = "SELECT * FROM user";

            return Db.Mapper.Query<User>(sql).ToList();
        }

        public static User GetById(uint id)
        {
            var sql = "SELECT * FROM user WHERE id = @id";

            return Db.Mapper.Query<User>(sql, new { id }).FirstOrDefault();
        }

        public static User GetByUsername(string username)
        {
            string sql = "SELECT * FROM user WHERE username = @username";

            return Db.Mapper.Query<User>(sql, new { username }).FirstOrDefault();
        }

        public static void Insert(User user)
        {
            var sql = @"INSERT INTO `user`
                        (
                            `username`,
                            `password`,
                            `firstName`,
                            `lastName`,
                            `email`,
                            `birthday`,
                            `mobile`,
                            `is_active`,
                            `created_at`,
                            `editor_id`,
                            `edited_at`
                        )VALUES(
                            @username,
                            @firstName,
                            @lastName,
                            @email,
                            @birthday,
                            @mobile,
                            @isActive,
                            NOW(),
                            @editorId,
                            NOW()

                        )

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                id = user.Id,
                username = user.Username,
                password = user.Password,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                birthday = user.Birthday,
                mobile = user.Mobile,
                isActive = user.IsActive,
                editorId = user.EditorId
            };

            user.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public static void Update(User user)
        {
            var sql = @"UPDATE `user`
                            SET username = @username,
                                first_name = @firstName,
                                last_name = @lastName,
                                email = @email,
                                birthday = @birthday,
                                mobile = @mobile,
                                is_active = @isActive,
                                editor_id = @editorId,
                                edited_at = NOW()
                        WHERE id = @id";

            var queryParams = new
            {
                id = user.Id,
                username = user.Username,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                birthday = user.Birthday,
                mobile = user.Mobile,
                isActive = user.IsActive,
                editorId = user.EditorId
            };

            Db.Mapper.Execute(sql, queryParams);
        }

        public static void Delete(uint id)
        {
            var sql = "DELETE FROM `user` WHERE id = @id";

            Db.Mapper.Query(sql, new { id });
        }
    }
}