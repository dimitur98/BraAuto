using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Conditions : BaseDal<Condition>
    {
        public Conditions() : base("condition", "id", "sort_order") { }
    }
}
