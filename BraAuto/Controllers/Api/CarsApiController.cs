using BraAuto.Helpers.Extensions;
using BraAuto.ViewModels;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/Cars/[action]")]
    public class CarsApiController : BaseApiController
    {
        [HttpGet]
        public List<SimpleCarModel> GetComparedCars()
        {
            var carIds = Db.UserCars.GetByUserId(this.LoggedUser.Id, Db.UserCarTypes.CompareId).Select(uc => uc.CarId).ToList();
            var result = new List<SimpleCarModel>();

            if (!carIds.IsNullOrEmpty())
            {
                var cars = Db.Cars.GetByIds(carIds);
                Db.Cars.LoadModels(cars);
                Db.Models.LoadMakes(cars.Select(c => c.Model));

                foreach (var car in cars)
                {
                    var carImg = Db.CarImgs.GetByCarId(car.Id).FirstOrDefault();
                    result.Add( new SimpleCarModel
                    {
                        Id = car.Id,
                        MakeModel = car.Model.Make.Name + " " + car.Model.Name,
                        ImgUrl = carImg?.Url
                    });
                }
            }

            return result;
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public IActionResult StatusChange(uint id, bool status)
        {
            var car = Db.Cars.GetById(id);

            if (car == null) { return this.NotFound(); }

            car.IsApproved = status;

            Db.Cars.Update(car);

            return this.Ok(new { });
        }
    }
}
