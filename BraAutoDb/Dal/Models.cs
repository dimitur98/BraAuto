using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Models : BaseDal<Model>
    {
        public Models() : base("model", "id", "sort_order") { }

        public List<Model> GetByMakeId(uint makeId) => this.GetByFieldValues("make_id", new uint[] { makeId });

        public void LoadMakes(IEnumerable<Model> models)
        {
            Db.LoadEntities(models, m => m.MakeId, makeIds => Db.Makes.GetByIds(makeIds), (car, makes) => car.Make = makes.FirstOrDefault(m => m.Id == car.MakeId));
        }
    }
}
