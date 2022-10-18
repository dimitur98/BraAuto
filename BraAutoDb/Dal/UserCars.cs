using BraAuto.Helpers.Extensions;
using BraAutoDb.Models;
using BraAutoDb.Models.UserCarsSearch;

namespace BraAutoDb.Dal
{
    public class UserCars : BaseDal<UserCar>
    {
        public UserCars() : base("user_car", "id", "id") { }
        
        public Response Search(Request request)
        {
            return this.Search<Response>(request,
                (query) =>
                {
                    if (!request.CarIds.IsNullOrEmpty()) { query.Where.Add(" AND uc.car_id IN @carIds"); }
                    if (!request.UserCarTypeIds.IsNullOrEmpty()) { query.Where.Add(" AND uc.user_car_type_id IN @userCarTypeIds"); }
                    if (request.UserId != null) { query.Where.Add(" AND uc.user_id = @userId"); }
                    if (request.Date != null) { query.Where.Add(" AND DATE(@date) = DATE(uc.date)"); }
                    if (request.CreatorId != null) { query.Where.Add(" AND uc.creator_id = @creatorId"); }

                    query.AddJoinIf("LEFT JOIN car c ON uc.car_id = c.id", orderByColumnStartsWith: "c.");
                },
                () =>
                {
                    return new
                    {
                        carIds = request.CarIds,
                        userCarTypeIds = request.UserCarTypeIds,
                        userId = request.UserId,
                        date = request.Date,
                        creatorId = request.CreatorId
                    };
                },
                "uc");
        }

        public List<(uint CarId, int Count)> GetCount(uint userCarTypeId)
        {
            var sql = @"
                    SELECT car_id, COUNT(creator_id) AS 'Count' FROM user_car
                    WHERE user_car_type_id = @userCarTypeId
                    GROUP BY car_id";

            return Db.Mapper.Query<(uint CarId, int Count)>(sql, new { userCarTypeId }).ToList();
        }

        public void Insert(UserCar userCar)
        {
            var sql = @"INSERT INTO `user_car`
                        (
                            `user_id`,
                            `car_id`,
                            `user_car_type_id`,
                            `date`,
                            `description`,
                            `creator_id`
                        )VALUES(
                            @userId,
                            @carId,
                            @userCarTypeId,
                            @date,
                            @description,
                            @creatorId
                        );

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                userId = userCar.UserId,
                carId = userCar.CarId,
                userCarTypeId = userCar.UserCarTypeId,
                date = userCar.Date,
                description = userCar.Description,
                creatorId = userCar.CreatorId
            };

            userCar.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public void Update(UserCar userCar)
        {
            var sql = @"UPDATE `user_car`
                            SET user_car_type_id = @userCarTypeId
                        WHERE id = @id";
            var queryParams = new
            {
                id = userCar.Id,
                userCarTypeId = userCar.UserCarTypeId
            };

            Db.Mapper.Execute(sql, queryParams);
        }

        public void Delete(uint carId, uint creatorId, IEnumerable<uint> userCarTypeIds)
        {
            string sql = $@"
                DELETE FROM `{_table}` 
                WHERE car_id = @carId AND creator_id = @creatorId ";

            if (!userCarTypeIds.IsNullOrEmpty()) { sql += " AND user_car_type_id IN @userCarTypeIds"; }

            Db.Mapper.Execute(sql, new { carId, userCarTypeIds, creatorId });
        }

        public void LoadCars(IEnumerable<UserCar> userCar)
        {
            Db.LoadEntities(userCar, uc => uc.CarId, carIds => Db.Cars.GetByIds(carIds), (userCar, cars) => userCar.Car = cars.FirstOrDefault(c => c.Id == userCar.CarId));
        }
    }
}
