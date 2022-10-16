using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/Models/[action]")]
    public class ModelsApiController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public List<SimpleSearchModel> GetByMakeId(uint? makeId = null, uint vehicleTypeId = 1)
        {
            if (makeId == null) { return new List<SimpleSearchModel>(); }

            return Db.Models.GetByMakeId(makeId.Value, vehicleTypeId).Select(m => new SimpleSearchModel(m.Id, m.Name)).ToList();
        }
    }
}
