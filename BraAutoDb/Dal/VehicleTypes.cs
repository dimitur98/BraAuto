using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class VehicleTypes : BaseDal<VehicleType>
    {
        public VehicleTypes() : base("vehicle_type", "id", "sort_order") { }
    }
}
