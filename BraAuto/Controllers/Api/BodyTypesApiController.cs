using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/BodyTypes/[action]")]
    public class BodyTypesApiController : BaseApiController
    {
        [AllowAnonymous]
        public List<SimpleSearchModel> GetAll(uint vehicleTypeId = 1)
        {
            return Db.BodyTypes.GetAll(vehicleTypeId).Select(bt => new SimpleSearchModel(bt.Id, bt.Name)).ToList();
        }
    }
}
