using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Articles : BaseDal<Article>
    {
        public Articles() : base("article", "id", "created_at", sortDesc: true) { }

        public void Insert(Article article)
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

        public void Update(Article article)
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

        public void Delete(uint id) => this.Delete(id);
    }
}
