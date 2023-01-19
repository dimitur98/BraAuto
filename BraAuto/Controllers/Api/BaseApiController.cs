using BraAuto.Helpers.Log;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [ApiController]
    [Authorize]
    public class BaseApiController : ControllerBase
    {
        private ILogHelper _log;
        private IConfiguration _config;
        private User _loggedUser;

        protected ILogHelper Log => _log ??= HttpContext.RequestServices.GetService<ILogHelper>();
        protected IConfiguration Config => _config ??= HttpContext.RequestServices.GetService<IConfiguration>();

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
    }
}
