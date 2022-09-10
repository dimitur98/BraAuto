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
        public List<SimpleSearchModel> GetAll()
        {
            return Db.BodyTypes.GetAll().Select(bt => new SimpleSearchModel(bt.Id, bt.Name)).ToList();
        }
    }
}
