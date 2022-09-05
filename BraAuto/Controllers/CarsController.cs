using BraAuto.ViewModels;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers
{
    public class CarsController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Main()
        {
            var model = new CarMainSearchModel
            {
                Makes = Makes.GetAll()
            };

            return this.View(model);
        }

        [AllowAnonymous]
        public IActionResult Search(CarSearchModel model)
        {
            return this.View();
        }
    }
}
