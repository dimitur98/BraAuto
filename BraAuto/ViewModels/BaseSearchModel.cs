using BraAutoDb.Models.Search;
using System.ComponentModel;

namespace BraAuto.ViewModels
{
    public abstract class BaseSearchModel<TSearchResponse, T>
        where TSearchResponse : BaseResponse<T>
    {
        public string SortBy { get; set; } 
        public bool? SortDesc { get; set; }
        public bool ShowAllSortFields { get; set; } = true;

        public int? Page { get; set; }

        [DisplayName("Page size")]
        public int? RowCount { get; set; } = 15;

        public TSearchResponse Response { get; set; }

        public List<int> RowCounts { get; set; } = new List<int>() { 1,15, 50, 100, 200, 300, 500 };

        protected virtual void SetSearchRequest(BaseRequest request)
        {
            if (this.Page < 1) { this.Page = null; }
            if (this.RowCount < 1) { this.RowCount = 15; }

            request.RowCount = this.RowCount;
            request.Offset = ((this.Page ?? 1) - 1) * this.RowCount;

            if (!string.IsNullOrWhiteSpace(this.SortBy))
            {
                request.SortDesc = this.SortDesc == true;
                request.SortColumn = this.SortBy;
            }
        }

        public virtual void SetDefaultSort(string sortBy, bool? sortDesc = null)
        {
            if (string.IsNullOrEmpty(this.SortBy))
            {
                this.SortBy = sortBy;
                this.SortDesc = sortDesc;
            }
        }

        public virtual Pager ToPager(object createButtonRouteValues = null)
        {
            return new Pager(this.Page ?? 1, this.RowCount ?? 15, this.RowCounts, this.SortBy, this.SortDesc ?? false, this.ShowAllSortFields, this.Response.TotalRecords, createButtonRouteValues: createButtonRouteValues);
        }
    }
}
