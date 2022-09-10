using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Colors : BaseDal<Color>
    {
        public Colors() : base("color", "id", "sort_order") { }
    }
}
