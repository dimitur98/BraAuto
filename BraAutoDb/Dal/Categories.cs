using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Categories : BaseDal<Category>
    {
        public Categories() : base("category", "id", "sort_order") { }
    }
}
