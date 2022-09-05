using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public static class CarServices
    {
        public static CarService GetById(uint id)
        {
            var sql = @"
                SELECT * 
                FROM car_service 
                WHERE id = @id";

            return Db.Mapper.Query<CarService>(sql, new { id }).FirstOrDefault();
        }

        public static List<CarService> GetAll()
        {
            var sql = @"
                SELECT * 
                FROM car_service";

            return Db.Mapper.Query<CarService>(sql).ToList();
        }

        public static void Insert(CarService carService)
        {
            var sql = @"INSERT INTO `car_service`
                        (
                            `name`,
                            `description`,
                            `img`,
                            `location`,
                            `creator_id`,
                            `created_at`,
                            `editor_id`,
                            `edited_at`
                        )VALUES(
                            @name,
                            @description,
                            @img,
                            @location,
                            @creatorId,
                            NOW(),
                            @editorId,
                            NOW()
                        )

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                name = carService.Name,
                description = carService.Description,
                img = carService.Img,
                location = carService.Location,
                creatorId = 0,
                editorId = 0
            };

            carService.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public static void Update(CarService carService)
        {
            var sql = @"UPDATE `car_service`
                            SET name = @name,
                                description = @description,
                                img = @img,
                                location = @location,
                                editor_id = @editorId,
                                edited_at = NOW()
                        WHERE id = @id";

            var queryParams = new
            {
                id = carService.Id,
                name = carService.Name,
                description = carService.Description,
                img = carService.Img,
                location = carService.Location,
                editorId = 0
            };

            Db.Mapper.Execute(sql, queryParams);
        }

        public static void Delete(uint id)
        {
            var sql = "DELETE FROM `car_service` WHERE id = @id";

            Db.Mapper.Query(sql, new { id });
        }
    }
}
