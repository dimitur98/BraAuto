namespace BraAuto.ViewModels
{
    public class Pager
    {
        public Pager(long page, int pageSize, IEnumerable<int> pageSizes, string sortBy, bool sortDesc, bool showSpecificSortFields, IEnumerable<(string Name, string SortColumn, bool SortDesc, bool Specific)> sortFields, long totalItemsCount, object createButtonRouteValues = null)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.PageSizes = pageSizes;
            this.SortBy = sortBy;
            this.SortDesc = sortDesc;
            this.ShowSpecificSortFields = showSpecificSortFields;
            this.SortFields = sortFields;
            this.TotalItemsCount = totalItemsCount;
            this.CreateButtonRouteValues = createButtonRouteValues;
        }

        public long TotalItemsCount { get; set; }
        public long Page { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<int> PageSizes { get; set; }
        public string SortBy { get; set; }
        public bool SortDesc { get; set; }
        public bool ShowSpecificSortFields { get; set; }
        public IEnumerable<(string Name, string SortColumn, bool SortDesc, bool Specific)> SortFields { get; set; }

        public int MaxDisplayedPages { get; set; } = 10;
        public object CreateButtonRouteValues { get; set; }


        public long PagesCount
        {
            get
            {
                var result = (long)Math.Ceiling(this.TotalItemsCount / (decimal)this.PageSize);

                return (result < 1) ? 1 : result;
            }
        }

        public IEnumerable<long> DisplayedPages
        {
            get
            {
                var page = this.Page;
                var pagesCount = this.PagesCount;
                var result = new List<long>();
                var start = Math.Max(page - (long)Math.Floor(this.MaxDisplayedPages / (decimal)2), 1);
                var end = Math.Min((start - 1) + this.MaxDisplayedPages, pagesCount);

                for (var i = start; i <= end; i++)
                {
                    result.Add(i);
                }

                return result;
            }
        }
    }
}
