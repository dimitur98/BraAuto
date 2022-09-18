using BraAutoDb.Models.Search;

namespace BraAutoDb.Models.ArticlesSearch
{
    public class Request : BaseRequest
    {
        public string Keywords { get; set; }

        public uint? CategoryId { get; set; }

        public bool? IsApproved { get; set; }
    }
}
