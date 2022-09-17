using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using System.Text.Encodings.Web;

namespace BraAuto.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("flatpickr")]
    [HtmlTargetElement("flatpickr", Attributes = ConfigAttributeName)]
    [HtmlTargetElement("flatpickr", Attributes = ShowTimeAttributeName)]
    [HtmlTargetElement("flatpickr", Attributes = ShowDayAttributeName)]
    public class FlatPickrTagHelper : InputTagHelper
    {
        private const string ConfigAttributeName = "fp-config";
        private const string ShowTimeAttributeName = "fp-show-time";
        private const string ShowDayAttributeName = "fp-show-day";

        private readonly IConfiguration Configuration;
        private readonly string _configDateJavascript;
        private readonly string _configDateDotNetFormat;
        private readonly string _configDateTimeJavascript;
        private readonly string _configDateTimeDotNetFormat;
        private readonly string _configDateMonthJavascript;
        private readonly string _configDateMonthDotNetFormat;

        [HtmlAttributeName(ConfigAttributeName)]
        public string Config { get; set; }

        [HtmlAttributeName(ShowTimeAttributeName)]
        public bool ShowTime { get; set; }

        [HtmlAttributeName(ShowDayAttributeName)]
        public bool ShowDay { get; set; } = true;

        public FlatPickrTagHelper(IHtmlGenerator generator, IConfiguration configuration) : base(generator)
        {
            this.Configuration = configuration;

            _configDateDotNetFormat = this.Configuration.GetSection("FlatPickr:Date:DotNetFormat").Value;
            _configDateJavascript = this.Configuration.GetSection("FlatPickr:Date:Config").Value;
            _configDateTimeDotNetFormat = this.Configuration.GetSection("FlatPickr:DateTime:DotNetFormat").Value;
            _configDateTimeJavascript = this.Configuration.GetSection("FlatPickr:DateTime:Config").Value;
            _configDateMonthDotNetFormat = this.Configuration.GetSection("FlatPickr:DateMonth:DotNetFormat").Value;
            _configDateMonthJavascript = this.Configuration.GetSection("FlatPickr:DateMonth:Config").Value;

            if (string.IsNullOrEmpty(_configDateDotNetFormat))
            {
                _configDateDotNetFormat = "{0:yyyy-MM-dd}";
            }

            if (string.IsNullOrEmpty(_configDateTimeDotNetFormat))
            {
                _configDateTimeDotNetFormat = "{0:yyyy-MM-dd HH:mm}";
            }

            if (string.IsNullOrEmpty(_configDateMonthDotNetFormat))
            {
                _configDateTimeDotNetFormat = "{0:yyyy-MM-01}";
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";

            // set format
            if (string.IsNullOrEmpty(this.Format))
            {
                this.Format = this.ShowTime ? _configDateTimeDotNetFormat : this.ShowDay ?_configDateDotNetFormat : _configDateMonthDotNetFormat;
            }

            // process the base input 
            base.Process(context, output);

            // set fieldId
            var fieldId = context.AllAttributes["id"]?.Value.ToString();

            if (string.IsNullOrEmpty(fieldId))
            {
                fieldId = output.Attributes["id"]?.Value.ToString();
            }

            if (string.IsNullOrEmpty(fieldId) && this.For != null)
            {
                fieldId = this.For.Name;

                output.Attributes.SetAttribute("id", fieldId);
            }

            if (string.IsNullOrEmpty(this.Config))
            {
                this.Config = this.ShowTime ? _configDateTimeJavascript : this.ShowDay ? _configDateJavascript : _configDateMonthJavascript;
            }

            var script = @"
                    <script type='text/javascript' language='javascript'>    
                        $(document).ready(function() {
                            flatpickr('#" + fieldId + @"', {
                                " + this.Config + @"
                            });
                         });
                    </script>                                    
                ";

            output.AddClass(this.ShowTime ? "datetime-flatpickr" : this.ShowDay ? "date-flatpickr" : "datemonth-flatpickr", htmlEncoder: HtmlEncoder.Default);
            output.Attributes.SetAttribute("type", "text");
            output.Attributes.SetAttribute("readonly", "readonly");
            output.Attributes.SetAttribute("autocomplete", "off");

            output.PostElement.AppendHtml(script);
        }
    }
}
