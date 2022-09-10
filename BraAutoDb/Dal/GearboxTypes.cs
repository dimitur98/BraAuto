using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class GearboxTypes : BaseDal<GearboxType>
    {
        public GearboxTypes() : base("gearbox_type", "id", "sort_order") { }
    }
}
