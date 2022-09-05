using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Helpers.Extensions
{
    public static class IUrlHelperExtensions
    {
        public static string BaseUrl(this IUrlHelper helper, bool addTrailingSlash = true)
        {
            var request = helper.ActionContext.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            var url = $"{request.Scheme}://{host}{pathBase}";

            if (addTrailingSlash == false && url.EndsWith("/")) { url = url.Substring(0, url.Length - 1); }

            return url;
        }
    }
}
