using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Makes : BaseDal<Make>
    {
        public Makes() : base("make", "id", "sort_order") { }
    }
}
