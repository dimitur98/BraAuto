using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class UserCars : BaseDal<UserCar>
    {
        public UserCars() : base("user_car", "id", "id") { }

        public IEnumerable<UserCar> GetByUserId(uint userId, uint? userCarTypeId = null)
        {
            string sql = $@"
                SELECT * 
                FROM `{_table}`
                WHERE user_id = @userId";

            if (userCarTypeId != null) { sql += " AND user_car_type_id = @userCarTypeId"; }

            return Db.Mapper.Query<UserCar>(sql, param: new { userId, userCarTypeId }).ToList();
        }
        public IEnumerable<(uint CarId, int Count)> GetCount()
        {
            var sql = @"
                    SELECT car_id, COUNT(user_id) AS 'Count' FROM user_car 
                    GROUP BY car_id";

            return Db.Mapper.Query<(uint CarId, int Count)>(sql).ToList();
        }

        public void Insert(UserCar userCar)
        {
            var sql = @"INSERT INTO `user_car`
                        (
                            `user_id`,
                            `car_id`,
                            `user_car_type_id`
                        )VALUES(
                            @userId,
                            @carId,
                            @userCarTypeId
                        );

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                userId = userCar.UserId,
                carId = userCar.CarId,
                userCarTypeId = userCar.UserCarTypeId
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
