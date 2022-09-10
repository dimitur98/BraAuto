using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/Locations/[action]")]
    public class LocationsApiController : BaseApiController
    {
        [AllowAnonymous]
        public List<SimpleSearchModel> GetAll()
        {
            return Db.Locations.GetAll().Select(l => new SimpleSearchModel(l.Id, l.Name)).ToList();
        }
    }
}
