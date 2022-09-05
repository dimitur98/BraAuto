using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class Articles
    {
        public static Article GetById(uint id)
        {
            var sql = "SELECT * FROM article WHERE id = @id";

            return Db.Mapper.Query<Article>(sql, new { id }).FirstOrDefault();
        }

        public static List<Article> GetAll()
        {
            var sql = "SELECT * FROM article";

            return Db.Mapper.Query<Article>(sql).ToList();
        }

        public static void Insert(Article article)
        {
            var sql = @"INSERT INTO `article`
                        (
                            `title`,
                            `body`,
                            `creator_id`,
                            `created_at`,
                            `editor_id`,
                            `edited_at`
                        )VALUES(
                            @title,
                            @body,
                            @creatorId,
                            NOW(),
                            @editorId,
                            NOW()
                        )

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                title = article.Title,
                body = article.Body,
                creatorId = 0,
                editorId = 0
            };

            article.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public static void Update(Article article)
        {
            var sql = @"UPDATE `article`
                            SET title = @title,
                                body = @body,
                                editor_id = @editorId,
                                edited_at = NOW()
                        WHERE id = @id";
            var queryParams = new
            {
                id = article.Id,
                title = article.Title,
                body = article.Body,
                editorId = 0
            };

            Db.Mapper.Execute(sql, queryParams);
        }

        public static void Delete(uint id)
        {
            var sql = "DELETE FROM `article` WHERE id = @id";

            Db.Mapper.Query(sql, new { id });
        }
    }
}
