using BraAutoDb.Models.Search;

namespace BraAutoDb.Models.UsersSearch
{
    public class Request : BaseRequest
    {
        public string Keywords { get; set; }

        public uint? UserTypeId { get; set; }

        public uint? LocationId { get; set; }
    }
}
