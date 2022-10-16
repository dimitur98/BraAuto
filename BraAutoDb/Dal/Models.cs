using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Models : BaseDal<Model>
    {
        public Models() : base("model", "id", "sort_order") { }

        public List<Model> GetByMakeId(uint makeId, uint vehicleTypeId)
        {
            var sql = @"
                SELECT *
                FROM model
                WHERE make_id = @makeId AND vehicle_type_id = @vehicleTypeId";

            return Db.Mapper.Query<Model>(sql, new { makeId, vehicleTypeId }).ToList();
        }

        public void LoadMakes(IEnumerable<Model> models)
        {
            Db.LoadEntities(models, m => m.MakeId, makeIds => Db.Makes.GetByIds(makeIds), (car, makes) => car.Make = makes.FirstOrDefault(m => m.Id == car.MakeId));
        }
    }
}
