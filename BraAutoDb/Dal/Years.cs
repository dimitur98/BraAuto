using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Years : BaseDal<Year>
    {
        public Years() : base("year", "id", "sort_order", sortDesc: true) { }
    }
}
