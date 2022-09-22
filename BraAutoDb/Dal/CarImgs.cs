using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class CarImgs : BaseDal<CarImg>
    {
        public CarImgs() : base("car_img", "id", "sort_order") { }

        public List<CarImg> GetByCarId(uint carId) => this.GetByFieldValues("car_id", new uint[] { carId });

        public List<CarImg> GetByCarIds(IEnumerable<uint> carIds) => this.GetByFieldValues("car_id", carIds);

        public void Insert(CarImg carImg)
        {
            var sql = @"INSERT INTO `car_img`
                        (
                            `url`,
                            `car_id`,
                            `sort_order`
                        )VALUES(
                            @url,
                            @carId,
                            @sortOrder
                        );

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                url = carImg.Url,
                carId = carImg.CarId,
                sortOrder = carImg.SortOrder
            };

            carImg.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public void DeleteByCarId(uint carId) => this.DeleteByField("car_id", new uint[] { carId });

        //public void Update(CarImg carImg)
        //{
        //    var sql = @"UPDATE `car_img`
        //                    SET url = @url,
        //                        car_id = @carId,
        //                        sort_order = @sortOrder
        //                WHERE id = @id";
        //    var queryParams = new
        //    {
        //        id = carImg.Id,
        //        url = carImg.Url,
        //        carId = carImg.CarId,
        //        sortOrder = carImg.SortOrder
        //    };

        //    Db.Mapper.Execute(sql, queryParams);
        //}
    }
}
