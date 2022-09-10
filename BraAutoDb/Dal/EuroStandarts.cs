using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class EuroStandarts : BaseDal<EuroStandart>
    {
        public EuroStandarts() : base("euro_standart", "id", "sort_order") { }
    }
}
