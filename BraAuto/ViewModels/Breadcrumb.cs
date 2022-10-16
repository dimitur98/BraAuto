namespace BraAuto.ViewModels
{
    public class Breadcrumb
    {
        public Breadcrumb(IEnumerable<(string Action, string Controller)> paths, long? totalRecords = null)
        {
            this.Paths = paths;
            this.TotalRecords = totalRecords;
        }

        public IEnumerable<(string Action, string Controller)> Paths { get; set; }

        public long? TotalRecords { get; set; }
    }
}