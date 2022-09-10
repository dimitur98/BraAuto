using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;

namespace BraAuto.AspNetCore.Mvc.TagHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;lazymultiselect&gt; elements
    /// </summary>   
    [HtmlTargetElement("lazymultiselect")]
    [HtmlTargetElement("lazymultiselect", Attributes = ConfigAttributeName)]
    [HtmlTargetElement("lazymultiselect", Attributes = RequestUrlAttributeName)]
    public class LazyMultiselectTagHelper : SelectTagHelper
    {
        private const string ConfigAttributeName = "lms-config";
        private const string RequestUrlAttributeName = "lms-request-url";
        private const string MapFuncAttributeName = "lms-map-func";

        /// <summary>
        /// Lazymultiselect options like: buttonText: 'Selected', noneSelectedText: '-- All --', buttonClass: 'form-control form-control-sm'
        /// </summary>
        [HtmlAttributeName(ConfigAttributeName)]
        public string Config { get; set; }

        /// <summary>
        /// Lazymultiselect url to take the data from
        /// </summary>
        [HtmlAttributeName(RequestUrlAttributeName)]
        public string RequestUrl { get; set; }

        /// <summary>
        /// Lazymultiselect javascript function to mapt the <see cref="RequestUrl"/> results like: "response.map(function(item) { return { value: item.id, label: item.name }; })"
        /// </summary>
        [HtmlAttributeName(MapFuncAttributeName)]
        public string MapFunc { get; set; }

        /// <summary>
        /// Creates a new <see cref="LazyMultiselectTagHelper"/>.
        /// </summary>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public LazyMultiselectTagHelper(IHtmlGenerator generator) : base(generator) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            output.Attributes.SetAttribute("multiple", "multiple");
            if (!output.Attributes.ContainsName("size")) { output.Attributes.SetAttribute("size", 1); }


            // set select items
            if (this.Items == null && this.For.Model != null && this.For.Model is IEnumerable)
            {
                this.Items = new MultiSelectList((IEnumerable)this.For.Model, (IEnumerable)this.For.Model);
            }

            // process the base select 
            base.Process(context, output);


            // set fieldId
            var fieldId = this.For?.Name;

            if (fieldId == null && context.AllAttributes["id"] != null)
            {
                fieldId = context.AllAttributes["id"].Value.ToString();
            }

            if (fieldId == null)
            {
                fieldId = context.UniqueId;

                output.Attributes.SetAttribute("id", fieldId);
            }


            // create javascript
            var config = !string.IsNullOrWhiteSpace(this.Config) ? this.Config + ", " : string.Empty;

            if (!string.IsNullOrWhiteSpace(this.RequestUrl) && !string.IsNullOrWhiteSpace(this.MapFunc))
            {
                config += string.Format(@"
                        source: function(callback) {{ 
                            $.ajax({{
                                url: '{0}',
                                type:'GET',
                                success: function (response, status, xhr) {{
                                    var items = {1};

                                    callback(items);
                                }},
                                notification: null,
                                enableRequestAbortedErrorHandling: false
                            }});
                        }}
                 ",
                 this.RequestUrl,
                 this.MapFunc);
            }

            string script = @"
                    <script type='text/javascript' language='javascript'>    
                        $(document).ready(function() {
                            $('#" + fieldId + @"').lazyMultiselect({
                                " + config + @"
                            });
                         });
                    </script>                                    
                ";

            output.PostElement.AppendHtml(script);
        }
    }
}
