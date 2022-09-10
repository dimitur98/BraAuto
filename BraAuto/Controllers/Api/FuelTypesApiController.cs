using BraAuto.ViewModels.Api.Common;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/FuelTypes/[action]")]
    public class FuelTypesApiController : BaseApiController
    {
        [AllowAnonymous]
        public List<SimpleSearchModel> GetAll()
        {
            return Db.FuelTypes.GetAll().Select(ft => new SimpleSearchModel(ft.Id, ft.Name)).ToList();
        }
    }
}
