//using BraAuto.ViewModels.Api.Common;
//using BraAutoDb.Dal;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace BraAuto.Controllers.Api
//{
//    [Route("/api/Years/[action]")]
//    public class YearsApiController : BaseApiController
//    {
//        [AllowAnonymous]
//        public List<SimpleSearchModel> GetAll()
//        {
//            return Years.GetAll().Select(y => new SimpleSearchModel(y.Id, y.Name)).ToList();
//        }
//    }
//}
