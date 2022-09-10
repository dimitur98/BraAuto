using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class CarInCarServices : BaseDal<CarInCarService>
    {
        public CarInCarServices() : base("car_in_car_service", "id", "id") { }

        public List<CarInCarService> GetByCarId(uint carId) => this.GetByFieldValues("car_id", new uint[] { carId });

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
    }
}
