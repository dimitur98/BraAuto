using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/Colors/[action]")]
    public class ColorsApiController : Controller
    {
        [AllowAnonymous]
        public List<SimpleSearchModel> GetAll()
        {
            return Db.Colors.GetAll().Select(c => new SimpleSearchModel(c.Id, c.Name)).ToList();
        }
    }
}
