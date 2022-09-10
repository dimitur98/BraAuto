using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class FuelTypes : BaseDal<FuelType>
    {
        public FuelTypes() : base("fuel_type", "id", "sort_order") { }
    }
}
