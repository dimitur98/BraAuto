namespace BraAuto.ViewModels
{
    public class Breadcrumb
    {
        public Breadcrumb(IEnumerable<(string Action, string Controller)> paths)
        {
            this.Paths = paths;
        }

        public IEnumerable<(string Action, string Controller)> Paths { get; set; }
    }
}