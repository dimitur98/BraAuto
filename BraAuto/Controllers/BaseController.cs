using BraAuto.Helpers.Extensions;
using BraAuto.Helpers.Log;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace BraAuto.Controllers
{
    public abstract class BaseController : Controller
    {
        private ILogHelper _log;
        private IConfiguration _config;
        private IWebHostEnvironment _webHostEnvironment;
        private IMemoryCache _memoryCache;
        private User _loggedUser;

        protected ILogHelper Log => _log ??= HttpContext.RequestServices.GetService<ILogHelper>();
        protected IConfiguration Config => _config ??= HttpContext.RequestServices.GetService<IConfiguration>();
        protected IWebHostEnvironment WebHostEnvironment => _webHostEnvironment ??= HttpContext.RequestServices.GetService<IWebHostEnvironment>();
        protected IMemoryCache MemoryCache => _memoryCache ??= HttpContext.RequestServices.GetService<IMemoryCache>();

        public BaseController()
        {
        }

        public User LoggedUser
        {
            get
            {
                if (_loggedUser == null)
                {
                    var username = this.User?.Identity?.Name;

                    if (!string.IsNullOrEmpty(username))
                    {
                        _loggedUser = Db.Users.GetByUsername(username);

                        if (_loggedUser != null && !_loggedUser.IsActive)
                        {
                            _loggedUser = null;
                        }
                    }
                }

                return _loggedUser;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            this.ViewBag.LoggedUser = this.LoggedUser;
            this.ViewBag.BuildVersion = this.GetType().Assembly.GetName().Version.ToString();

            var popup = this.HttpContext.Request.Query["popup"].ToString();

            if (popup != null && popup.ToLower() == "true")
            {
                this.ViewBag.ShowHeader = false;
                this.ViewBag.ShowFooter = false;
                this.ViewBag.IsPopup = true;
            }
        }

        protected IActionResult RedirectToReferrer(string defaultActionName = null, string defaultControllerName = null, object defaultRouteValues = null)
        {
            var requestHeaders = this.HttpContext.Request.GetTypedHeaders();
            var url = requestHeaders?.Referer != null ? new Uri(requestHeaders.Referer.ToString()).PathAndQuery : null;

            return this.RedirectToLocal(url, defaultActionName, defaultControllerName: defaultControllerName, defaultRouteValues: defaultRouteValues);
        }

        protected IActionResult RedirectToLocal(string url, string defaultActionName = null, string defaultControllerName = null, object defaultRouteValues = null)
        {
            var baseUrl = Url.BaseUrl(addTrailingSlash: false);

            url = url != null && url.StartsWith(baseUrl) ? url.Substring(baseUrl.Length) : url;

            if (!String.IsNullOrWhiteSpace(url) && Url.IsLocalUrl(url))
            {
                return Redirect(url);
            }
            else
            {
                return RedirectToAction(defaultActionName, defaultControllerName, defaultRouteValues);
            }
        }

        protected IActionResult RedirectToHttpNotFound()
        {
            return this.RedirectToAction("Status", "Errors", new { code = 404, path = this.Request.Path });
        }

        protected IActionResult RedirectToHttpForbidden()
        {
            return this.RedirectToAction("NoAccess", "Errors", new { path = this.Request.Path });
        }

        protected string GetRefererAbsoluteUri()
        {
            var requestHeaders = this.HttpContext.Request.GetTypedHeaders();

            return requestHeaders?.Referer?.AbsoluteUri;
        }
    }
}
