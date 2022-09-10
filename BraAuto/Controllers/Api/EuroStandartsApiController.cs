using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/EuroStandarts/[action]")]
    public class EuroStandartsApiController : Controller
    {
        [AllowAnonymous]
        public List<SimpleSearchModel> GetAll()
        {
            return Db.EuroStandarts.GetAll().Select(es => new SimpleSearchModel(es.Id, es.Name)).ToList();
        }
    }
}
