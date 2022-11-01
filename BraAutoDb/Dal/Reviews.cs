using BraAutoDb.Models;
using BraAutoDb.Models.ReviewsSearch;

namespace BraAutoDb.Dal
{
    public class Reviews : BaseDal<Review>
    {
        public Reviews() : base("review", "id", "first_name") { }

        public Response Search(Request request)
        {
            return this.Search<Response>(request,
                (query) => {},
                () =>
                {
                    return new {};
                },
                "r");
        }

        public void Insert(Review review)
        {
            var sql = @"INSERT INTO `review`
                        (
                            `first_name`,
                            `last_name`,
                            `description`,
                            `star_rating`,
                            `created_at`
                        )VALUES(
                            @firstName,
                            @lastName,
                            @description,
                            @starRating,
                            NOW()
                        );

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                firstName = review.FirstName,
                lastName = review.LastName,
                description = review.Description,
                starRating = review.StarRating
            };

            review.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }
    }
}
