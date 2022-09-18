using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/Users/[action]")]
    public class UsersApiController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "administrator")]
        public IActionResult StatusChange(uint id, bool status)
        {
            var user = Db.Users.GetById(id);

            if (user == null) { return this.NotFound(); }

            user.IsActive = status;

            Db.Users.Update(user);

            return this.Ok(new { });
        }
    }
}
