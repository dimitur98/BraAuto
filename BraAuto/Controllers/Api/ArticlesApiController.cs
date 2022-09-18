using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/Articles/[action]")]
    public class ArticlesApiController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles = "administrator")]
        public IActionResult StatusChange(uint id, bool status)
        {
            var article = Db.Articles.GetById(id);

            if (article == null) { return this.NotFound(); }

            article.IsApproved = status;

            Db.Articles.Update(article);

            return this.Ok(new { });
        }
    }
}
