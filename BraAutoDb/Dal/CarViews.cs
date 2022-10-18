using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class CarViews : BaseDal<CarView>
    {
        public CarViews() : base("car_view", "id", "car_id") { }

        public CarView Get(uint carId, string userIp)
        {
            var sql = @"
                SELECT *
                FROM car_view 
                WHERE car_id = @carId AND user_ip = @userIp";

            return Db.Mapper.Query<CarView>(sql, new { carId, userIp }).FirstOrDefault();
        }

        public int GetCountByCarId(uint carId)
        {
            var sql = @"
                SELECT COUNT(*) 
                FROM car_view 
                WHERE car_id = @carId";

            return Db.Mapper.Query<int>(sql, new { carId }).FirstOrDefault();
        }

        public void Insert(CarView carView)
        {
            var sql = @"INSERT INTO `car_view`
                        (
                            `car_id`,
                            `user_ip`,
                            `created_at`
                        )VALUES(
                            @carId,
                            @userIp,
                            NOW()
                        );

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                carId = carView.CarId,
                userIp = carView.UserIp,
            };

            carView.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }
    
        public void Delete(uint carId, string ip)
        {
            string sql = $@"
                DELETE FROM `{_table}` 
                WHERE car_id = @carId AND user_ip = @ip";

            Db.Mapper.Execute(sql, new { carId, ip });
        }
    }
}
