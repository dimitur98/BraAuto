using BraAutoDb.Models;
using BraAutoDb.Models.ArticlesSearch;

namespace BraAutoDb.Dal
{
    public class Articles : BaseDal<Article>
    {
        public Articles() : base("article", "id", "created_at", sortDesc: true) { }

        public Response Search(Request request)
        {
            return this.Search<Response>(request,
                (query) =>
                {
                    if (!string.IsNullOrEmpty(request.Keywords)) { query.Where.Add("AND (a.title LIKE @keywords)"); }
                    if (request.CategoryId != null) { query.Where.Add(" AND a.category_id = @categoryId"); }
                    if (request.IsApproved != null) { query.Where.Add(" AND a.is_approved = @isApproved"); }
                },
                () =>
                {
                    return new
                    {
                        keywords = string.Format("%{0}%", request.Keywords),
                        categoryId = request.CategoryId,
                        isApproved = request.IsApproved
                    };
                },
                "a");
        }

        public Article GetByTitle(string title)
        {
            var sql = @"
                SELECT *
                FROM article
                WHERE title LIKE @title";

            return Db.Mapper.Query<Article>(sql, new { title = string.Format("%{0}%", title)}).FirstOrDefault();
        }

        public void Insert(Article article)
        {
            var sql = @"INSERT INTO `article`
                        (
                            `title`,
                            `body`,
                            `category_id`,
                            `img_url`,
                            `is_approved`,
                            `creator_id`,
                            `created_at`,
                            `editor_id`,
                            `edited_at`
                        )VALUES(
                            @title,
                            @body,
                            @categoryId,
                            @imgUrl,
                            @isApproved,
                            @creatorId,
                            NOW(),
                            @editorId,
                            NOW()
                        );

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                title = article.Title,
                body = article.Body,
                categoryId = article.CategoryId,
                imgUrl = article.ImgUrl,
                isApproved = article.IsApproved,
                creatorId = article.CreatorId,
                editorId = article.EditorId
            };

            article.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public void Update(Article article)
        {
            var sql = @"UPDATE `article`
                            SET title = @title,
                                body = @body,
                                category_id = @categoryId,
                                img_url = @imgUrl,
                                is_approved = @isApproved,
                                editor_id = @editorId,
                                edited_at = NOW()
                        WHERE id = @id";
            var queryParams = new
            {
                id = article.Id,
                title = article.Title,
                body = article.Body,
                categoryId = article.CategoryId,
                imgUrl = article.ImgUrl,
                isApproved = article.IsApproved,
                editorId = article.EditorId
            };

            Db.Mapper.Execute(sql, queryParams);
        }

        public void LoadCreators(IEnumerable<Article> articles)
        {
            Db.LoadEntities(articles, a => a.CreatorId, creatorIds => Db.Users.GetByIds(creatorIds), (article, creators) => article.Creator = creators.FirstOrDefault(c => c.Id == article.CreatorId));
        }
    }
}
