namespace BraAutoDb.Models.Search
{
    public interface IBaseRequest
    {
        bool? IsActive { get; set; }
        string SortColumn { get; set; }
        bool SortDesc { get; set; }
        int? Offset { get; set; }
        int? RowCount { get; set; }
        bool ReturnRecords { get; set; }
        bool ReturnTotalRecords { get; set; }
    }
    public abstract class BaseRequest : IBaseRequest
    {
        public bool? IsActive { get; set; }

        public string SortColumn { get; set; }

        public bool SortDesc { get; set; }

        public int? Offset { get; set; }

        public int? RowCount { get; set; }

        public bool ReturnRecords { get; set; } = true;

        public bool ReturnTotalRecords { get; set; } = true;
    }
}
