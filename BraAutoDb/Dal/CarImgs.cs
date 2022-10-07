using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class CarPhotos : BaseDal<CarPhoto>
    {
        public CarPhotos() : base("car_photo", "id", "sort_order") { }

        public List<CarPhoto> GetByCarId(uint carId) => this.GetByFieldValues("car_id", new uint[] { carId });

        public List<CarPhoto> GetByCarIds(IEnumerable<uint> carIds) => this.GetByFieldValues("car_id", carIds);

        public void Insert(CarPhoto carPhoto)
        {
            var sql = @"INSERT INTO `car_photo`
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
                url = carPhoto.Url,
                carId = carPhoto.CarId,
                sortOrder = carPhoto.SortOrder
            };

            carPhoto.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public void DeleteByCarId(uint carId) => this.DeleteByField("car_id", new uint[] { carId });
    }
}
