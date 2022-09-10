using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Locations : BaseDal<Location>
    {
        public Locations() : base("location", "id", "sort_order") { }
    }
}
