using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers
{
    public class ErrorsController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Status(int? code = null)
        {
            return View(code);
        }

        [AllowAnonymous]
        public IActionResult NoAccess()
        {
            return View();
        }
    }
}
