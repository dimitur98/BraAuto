namespace BraAutoDb.Models.Search
{
    public abstract class BaseResponse<T>
    {
        public IEnumerable<T> Records { get; set; }
        public long TotalRecords { get; set; }

        public BaseResponse()
        {
            this.Records = new List<T>();
        }
    }
}
