using BraAuto.Helpers.Extensions;
using BraAuto.Resources;
using BraAuto.ViewModels;
using BraAuto.ViewModels.Helpers;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers
{
    [Authorize(Roles = "administrator, blogger")]
    public class ArticlesController : BaseController
    {
        public IActionResult My(MyArticleModel model)
        {
            model.UserIds = new uint[] { this.LoggedUser.Id };

            this.ExecuteSearch(model);

            return this.View(model);
        }

        [Authorize(Roles = "administrator")]
        public IActionResult Admin(ArticleAdminModel model)
        {
            this.ExecuteSearch(model);

            Db.Articles.LoadCreators(model.Response.Records);

            return this.View(model);
        }
        public IActionResult Search(ArticleSearchModel model)
        {
            model.Categories = Db.Categories.GetAll();
            model.IsApproved = true;

            this.ExecuteSearch(model);

            return View(model);
        }

        public IActionResult Create()
        {
            var model = new ArticleCreateModel
            {
                Categories = Db.Categories.GetAll()
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(ArticleCreateModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var article = Db.Articles.GetByTitle(model.Title);

                    if (article != null)
                    {
                        model.Categories = Db.Categories.GetAll();
                        this.ModelState.AddModelError(string.Empty, Global.ArticleTitleExists);

                        return this.View(model);
                    }

                    article = new Article
                    {
                        Title = model.Title,
                        Body = model.Body,
                        CategoryId = model.CategoryId,
                        CreatorId = this.LoggedUser.Id,
                        EditorId = this.LoggedUser.Id
                    };

                    Db.Articles.Insert(article);

                    return this.RedirectToAction(nameof(Search));
                }
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            model.Categories = Db.Categories.GetAll();

            return this.View(model);
        }

        public IActionResult Edit(uint id)
        {
            var article = Db.Articles.GetById(id);

            if(article == null) { return this.NotFound(); }

            var model = new ArticleEditModel
            {
                Id = article.Id,
                Title = article.Title,
                Body = article.Body,
                CategoryId = article.CategoryId,
                Categories = Db.Categories.GetAll()
                
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Edit(ArticleEditModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var article = Db.Articles.GetById(model.Id);

                    if (article == null) { return this.NotFound(); }

                    article.Title = model.Title;
                    article.Body = model.Body;
                    article.CategoryId = model.CategoryId;
                    article.EditorId = this.LoggedUser.Id;

                    Db.Articles.Update(article);

                    return this.RedirectToAction(nameof(Search));
                }
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            model.Categories = Db.Categories.GetAll();

            return this.View(model);
        }

        public IActionResult Delete(uint id)
        {
            try
            {
                var article = Db.Articles.GetById(id);

                if (article == null) { return this.NotFound(); }

                if (!this.LoggedUser.IsAdmin() || this.LoggedUser.Id != article.CreatorId) { return this.RedirectToHttpForbidden(); }

                Db.Articles.Delete(id);

                this.TempData[Global.AlertKey] = new Alert(Global.ItemDeleted, AlertTypes.Info);
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            return this.RedirectToAction(nameof(My));
        }

        public IActionResult Details(uint id)
        {
            var article = Db.Articles.GetById(id);

            if(article == null) { return this.NotFound(); }

            article.LoadCreator();
            article.LoadCategory();

            var model = new ArticleDetailsModel
            {
                Title = article.Title,
                Body = article.Body,
                Category = article.Category,
                Creator = article.Creator,
                CreatedAt = article.CreatedAt
            };

            return this.View(model);
        }

        protected void ExecuteSearch(ArticleSearchBaseModel model)
        {
            model.SetDefaultSort("a.created_at", sortDesc: true);

            var request = model.ToSearchRequest();

            request.ReturnTotalRecords = true;

            var response = Db.Articles.Search(request);

            if (!response.Records.IsNullOrEmpty())
            {
                Db.Articles.LoadCreators(response.Records);
            }

            model.Response = response;
        }
    }
}
