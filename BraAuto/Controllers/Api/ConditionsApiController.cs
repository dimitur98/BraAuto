using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/Conditions/[action]")]
    public class ConditionsApiController : BaseApiController
    {
        [AllowAnonymous]
        public List<SimpleSearchModel> GetAll()
        {
            return Db.Conditions.GetAll().Select(c => new SimpleSearchModel(c.Id, c.Name)).ToList();
        }
    }
}
