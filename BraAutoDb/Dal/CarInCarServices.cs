using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class CarInCarServices
    {
        public static CarInCarService GetById(uint id)
        {
            var sql = "SELECT * FROM car_in_car_service WHERE id = @id";

            return Db.Mapper.Query<CarInCarService>(sql, new { id }).FirstOrDefault();
        }

        public static List<CarInCarService> GetByCarId(uint carId)
        {
            var sql = "SELECT * FROM car_in_car_service WHERE car_id = @carId";

            return Db.Mapper.Query<CarInCarService>(sql, new { carId }).ToList();
        }

        public static void Insert(CarInCarService carInCarService)
        {
            var sql = @"INSERT INTO `car_in_car_service`
                        (
                            `car_id`,
                            `car_service_id`,
                            `stage_id`
                        )VALUES(
                            @carId,
                            @carServiceId,
                            @stageId
                        )

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                carId = carInCarService.CarId,
                carServiceId = carInCarService.CarServiceId,
                stageId = carInCarService.StageId
            };

            carInCarService.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public static void Update(CarInCarService carInCarService)
        {
            var sql = @"UPDATE `car_in_car_service`
                            SET title = @title,
                                car_id = @carId,
                                car_service_id = @carServiceId,
                                stage_id = @stageId
                        WHERE id = @id";

            var queryParams = new
            {
                carId = carInCarService.CarId,
                carServiceId = carInCarService.CarServiceId,
                stageId = carInCarService.StageId
            };

            Db.Mapper.Execute(sql, queryParams);
        }

        public static void Delete(uint id)
        {
            var sql = "DELETE FROM `car_in_car_service` WHERE id = @id";

            Db.Mapper.Query(sql, new { id });
        }
    }
}
