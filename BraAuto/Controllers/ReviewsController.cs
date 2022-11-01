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
    public class ReviewsController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Search(ReviewSearchModel model)
        {
            this.ExecuteSearch(model);

            return View(model);
        }

        public IActionResult Create(ReviewCreateModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var review = new Review
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Description = model.Description,
                        StarRating = model.StarRating.Value
                    };

                    Db.Reviews.Insert(review);
                }
                else
                {
                    this.TempData[Global.AlertKey] = new Alert(Global.AllFieldsRequired, AlertTypes.Danger).SerializeAlert();
                }
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.TempData[Global.AlertKey] = new Alert(Global.SelectedTimeNotFree, AlertTypes.Danger).SerializeAlert();
            }

            return this.RedirectToAction(nameof(Search));
        }

        [Authorize(Roles = "administrator")]
        public IActionResult Delete(uint id)
        {
            try
            {
                var review = Db.Reviews.GetById(id);

                if (review == null) { return this.NotFound(); }

                Db.Reviews.Delete(id);
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.TempData[Global.AlertKey] = new Alert(Global.SelectedTimeNotFree, AlertTypes.Danger).SerializeAlert();
            }

            return this.RedirectToAction(nameof(Search));
        }

        protected void ExecuteSearch(ReviewSearchModel model)
        {
            model.SetDefaultSort("r.created_at", sortDesc: true);

            model.Response = Db.Reviews.Search(model.ToSearchRequest());
        }
    }
}
