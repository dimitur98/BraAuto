using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;

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

        public static string CurrentAction(this IUrlHelper helper)
        {
            var routeData = helper.ActionContext.RouteData;

            return (routeData.Values.ContainsKey("action") ? routeData.Values["action"].ToString() : null);
        }

        public static string Action(this IUrlHelper helper, string actionName, string controllerName, NameValueCollection queryString)
        {
            return helper.Action(actionName, controllerName) + queryString.ToQueryString();
        }

        public static string SortUrl(this IUrlHelper helper, string sortBy, bool? sortDesc = null, string sortByParamName = "sortBy", string sortDescParamName = "sortDesc", bool preserveQueryStringValues = true)
        {
            var queryString = helper.ActionContext.HttpContext.Request.Query.ToNameValueCollection();
            string currSortBy = queryString[sortByParamName];
            string currSortDesc = queryString[sortDescParamName];

            if (!sortDesc.HasValue)
            {
                sortDesc = sortBy == currSortBy && (currSortDesc == null || currSortDesc.ToLower() != "true") ? true : false;
            }

            queryString.AddOrSet(sortByParamName, sortBy, ignoreCase: true);
            queryString.RemoveIfExists(sortDescParamName, ignoreCase: true);

            if (sortDesc.Value)
            {
                queryString.AddOrSet(sortDescParamName, sortDesc.Value.ToString().ToLower(), ignoreCase: true);
            }

            return helper.Action(null, null, queryString: queryString);
        }
    }
}
