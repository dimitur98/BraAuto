using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class Stages : BaseDal<Stage>
    {
        public Stages() : base("stage", "id", "sort_order") { }
    }
}
