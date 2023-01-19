using BraAuto.Helpers.Extensions;
using BraAuto.Helpers.Log;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BraAuto.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        private ILogHelper _log;
        private IConfiguration _config;
        private User _loggedUser;

        protected ILogHelper Log => _log ??= HttpContext.RequestServices.GetService<ILogHelper>();
        protected IConfiguration Config => _config ??= HttpContext.RequestServices.GetService<IConfiguration>();

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


        protected IActionResult RedirectToHttpForbidden()
        {
            return this.RedirectToAction("NoAccess", "Errors", new { path = this.Request.Path });
        }
    }
}
