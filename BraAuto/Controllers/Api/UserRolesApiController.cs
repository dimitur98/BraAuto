using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/UserRoles/[action]")]
    public class UserRolesApiController : BaseApiController
    {
        [Authorize]
        public List<SimpleSearchModel> GetAll()
        {
            return Db.UserRoles.GetAll().Select(ur => new SimpleSearchModel(ur.Id, ur.Name)).ToList();
        }
    }
}
