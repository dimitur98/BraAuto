using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/Makes/[action]")]
    public class MakesApiController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public List<SimpleSearchModel> GetAll(uint vehicleTypeId = 1)
        {
            return Db.Makes.GetAll(vehicleTypeId).Select(m => new SimpleSearchModel(m.Id, m.Name)).ToList();
        }
    }
}
