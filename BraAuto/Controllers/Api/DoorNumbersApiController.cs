using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/DoorNumbers/[action]")]
    public class DoorNumbersApiController : BaseApiController
    {
        [AllowAnonymous]
        public List<SimpleSearchModel> GetAll()
        {
            return Db.DoorNumbers.GetAll().Select(dn => new SimpleSearchModel(dn.Id, dn.Name)).ToList();
        }
    }
}
