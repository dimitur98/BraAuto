using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class BodyTypes : BaseDal<BodyType>
    {
        public BodyTypes() : base("body_type", "id", "sort_order") { }

        public List<BodyType> GetAll(uint vehicleTypeId) => this.GetByFieldValues("vehicle_type_id", new uint[] { vehicleTypeId });
    }
}
