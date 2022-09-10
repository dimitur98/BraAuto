using BraAutoDb.Models;

namespace BraAutoDb.Dal
{
    public class DoorNumbers : BaseDal<DoorNumber>
    {
        public DoorNumbers() : base("door_number", "id", "sort_order") { }
    }
}
