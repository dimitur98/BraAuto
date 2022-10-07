using BraAuto.Helpers.Extensions;
using BraAutoDb.Models;
using SqlQueryBuilder.MySql;
using System.Drawing;

namespace BraAutoDb.Dal
{
    public class UserCars : BaseDal<UserCar>
    {
        public UserCars() : base("user_car", "id", "id") { }

        public List<UserCar> Get(IEnumerable<uint> userCarTypeIds, uint ? carId = null, uint? userId = null, DateTime? date = null)
        {
            var query = new Query()
            {
                Select = new List<string>() { "*" },
                From = $"`{_table}`",
                Where = new List<string>() { "1=1" }
            };

            if (carId != null) { query.Where.Add(" AND car_id = @carId"); }
            if (!userCarTypeIds.IsNullOrEmpty()) { query.Where.Add(" AND user_car_type_id IN @userCarTypeIds"); }
            if (userId != null) { query.Where.Add(" AND user_id = @userId"); }
            if (date != null) { query.Where.Add(" AND DATE(@date) = DATE(date)"); }

            return Db.Mapper.Query<UserCar>(query.ToString(), param: new { userCarTypeIds, carId, userId, date}).ToList();
        }

        public IEnumerable<UserCar> GetByUserId(uint userId, uint? userCarTypeId = null)
        {
            string sql = $@"
                SELECT * 
                FROM `{_table}`
                WHERE user_id = @userId";

            if (userCarTypeId != null) { sql += " AND user_car_type_id = @userCarTypeId"; }

            return Db.Mapper.Query<UserCar>(sql, param: new { userId, userCarTypeId }).ToList();
        }
        public IEnumerable<(uint CarId, int Count)> GetCount(uint userCarTypeId)
        {
            var sql = @"
                    SELECT car_id, COUNT(user_id) AS 'Count' FROM user_car
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
                            `date`
                        )VALUES(
                            @userId,
                            @carId,
                            @userCarTypeId,
                            @date
                        );

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                userId = userCar.UserId,
                carId = userCar.CarId,
                userCarTypeId = userCar.UserCarTypeId,
                date = userCar.Date
            };

            userCar.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public void Delete(uint userId, uint carId, uint userCarTypeId)
        {
            string sql = $@"
                DELETE FROM `{_table}` 
                WHERE user_id = @userId AND car_id = @carId AND user_car_type_id = @userCarTypeId";

            Db.Mapper.Execute(sql, new { userId, carId, userCarTypeId });
        }
    }
}
