using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/GearboxTypes/[action]")]
    public class GearboxTypesApiController : BaseApiController
    {
        [AllowAnonymous]
        public List<SimpleSearchModel> GetAll()
        {
            return GearboxTypes.GetAll().Select(gt => new SimpleSearchModel(gt.Id, gt.Name)).ToList();
        }
    }
}
