﻿using BraAuto.Helpers.CloudinaryService;
using BraAuto.Helpers.Extensions;
using BraAuto.Resources;
using BraAuto.ViewModels;
using BraAuto.ViewModels.Helpers;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers
{
    [Authorize(Roles = "administrator, blogger")]
    public class ArticlesController : BaseController
    {
        private Cloudinary _cloudinary;

        public ArticlesController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

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

        [AllowAnonymous]
        public IActionResult Search(ArticleSearchModel model)
        {
            model.Categories = Db.Categories.GetAll();
            model.IsApproved = true;
            model.ShowSpecificSortFields = false;

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
        public async Task<IActionResult> Create(ArticleCreateModel model)
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

                    if (!model.Photo.IsValidPhoto())
                    {
                        model.Categories = Db.Categories.GetAll();
                        this.ModelState.AddModelError(string.Empty, Global.InvalidPhoto);

                        return this.View(model);
                    }

                    var photoUrl = await model.Photo.UploadPhotoAsync();

                    article = new Article
                    {
                        Title = model.Title,
                        Body = model.Body,
                        CategoryId = model.CategoryId,
                        PhotoUrl = photoUrl,
                        IsApproved = false,
                        CreatorId = this.LoggedUser.Id,
                        EditorId = this.LoggedUser.Id
                    };

                    Db.Articles.Insert(article);

                    return this.RedirectToAction(nameof(My));
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

        public IActionResult Edit(uint id, bool isAdminArticleEditPage)
        {
            var article = Db.Articles.GetById(id);

            if (article == null) { return this.NotFound(); }

            var model = new ArticleEditModel
            {
                Id = article.Id,
                Title = article.Title,
                Body = article.Body,
                CategoryId = article.CategoryId,
                PhotoUrl = article.PhotoUrl,
                IsAdminArticleEditPage = isAdminArticleEditPage,
                Categories = Db.Categories.GetAll()
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleEditModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var article = Db.Articles.GetById(model.Id);

                    if (article == null) { return this.NotFound(); }

                    if (model.Photo != null)
                    {
                        await CloudinaryService.DeletePhoto(article.PhotoUrl);

                        if (!model.Photo.IsValidPhoto())
                        {
                            model.Categories = Db.Categories.GetAll();
                            this.ModelState.AddModelError(string.Empty, Global.InvalidPhoto);

                            return this.View(model);
                        }

                        article.PhotoUrl = await model.Photo.UploadPhotoAsync();
                    }

                    article.Title = model.Title;
                    article.Body = model.Body;
                    article.CategoryId = model.CategoryId;
                    article.EditorId = this.LoggedUser.Id;

                    Db.Articles.Update(article);

                    return this.RedirectToAction(model.IsAdminArticleEditPage ? nameof(Admin) : nameof(My));
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

        public async Task<IActionResult> Delete(uint id)
        {
            try
            {
                var article = Db.Articles.GetById(id);

                if (article == null) { return this.NotFound(); }

                if (!this.LoggedUser.IsAdmin() || this.LoggedUser.Id != article.CreatorId) { return this.RedirectToHttpForbidden(); }

                Db.Articles.Delete(id);

                await CloudinaryService.DeletePhoto(article.PhotoUrl);

                this.TempData[Global.AlertKey] = new Alert(Global.ItemDeleted, AlertTypes.Info).SerializeAlert();
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            return this.RedirectToAction(nameof(My));
        }

        [AllowAnonymous]
        public IActionResult Details(uint id)
        {
            var article = Db.Articles.GetById(id);

            if (article == null) { return this.NotFound(); }

            article.LoadCreator();
            article.LoadCategory();

            var model = new ArticleDetailsModel
            {
                Title = article.Title,
                Body = article.Body,
                Category = article.Category,
                PhotoUrl = article.PhotoUrl,
                Creator = article.Creator,
                CreatedAt = article.CreatedAt
            };

            return this.View(model);
        }

        protected void ExecuteSearch(ArticleSearchBaseModel model)
        {
            model.SetDefaultSort("a.created_at", sortDesc: true);

            var response = Db.Articles.Search(model.ToSearchRequest());

            if (!response.Records.IsNullOrEmpty())
            {
                Db.Articles.LoadCreators(response.Records);
            }

            model.Response = response;
        }
    }
}
