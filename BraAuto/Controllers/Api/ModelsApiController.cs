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
        public List<SimpleSearchModel> GetByMakeId(uint? makeId = null)
        {
            if (makeId == null) { return new List<SimpleSearchModel>(); }
            var a = Models.GetByMakeId(makeId.Value).Select(m => new SimpleSearchModel(m.Id, m.Name)).ToList();
            return Models.GetByMakeId(makeId.Value).Select(m => new SimpleSearchModel(m.Id, m.Name)).ToList();
        }
    }
}
