using BraAutoDb.Models.Search;

namespace BraAutoDb.Models.UserCarsSearch
{
    public class Request : BaseRequest
    {
        public IEnumerable<uint> UserCarTypeIds { get; set; }

        public IEnumerable<uint> CarIds { get; set; }

        public uint? UserId { get; set; }

        public DateTime? Date { get; set; }

        public uint? CreatorId { get; set; }
    }
}
