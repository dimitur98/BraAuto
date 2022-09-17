namespace BraAuto.ViewModels.Helpers
{
    public enum AlertTypes
    {
        Info = 1,
        Success = 2,
        Warning = 3,
        Danger = 4
    }

    public class Alert
    {
        public string Text { get; set; }
        public AlertTypes Type { get; set; }
        public bool HtmlEncode { get; set; }

        public Alert(string text, AlertTypes type, bool htmlEncode = false)
        {
            this.Text = text;
            this.Type = type;
            this.HtmlEncode = htmlEncode;
        }
    }
}
