using BraAuto.Helpers.Extensions;
using BraAuto.ViewModels;
using BraAutoDb.Dal;
using BraAutoDb.Models.UserCarsSearch;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/Cars/[action]")]
    public class CarsApiController : BaseApiController
    {
        private Cloudinary _cloudinary;

        public CarsApiController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public List<SimpleCarModel> GetComparedCars()
        {
            var carIds = Db.UserCars.Search( new Request { UserCarTypeIds = new uint[] { Db.UserCarTypes.CompareId }, CreatorId = this.LoggedUser.Id }).Records.Select(uc => uc.CarId).ToList();
            var result = new List<SimpleCarModel>();

            if (!carIds.IsNullOrEmpty())
            {
                var cars = Db.Cars.GetByIds(carIds);
                Db.Cars.LoadModels(cars);
                Db.Models.LoadMakes(cars.Select(c => c.Model));

                foreach (var car in cars)
                {
                    var carPhoto = Db.CarPhotos.GetByCarId(car.Id).FirstOrDefault();
                    result.Add( new SimpleCarModel
                    {
                        Id = car.Id,
                        MakeModel = car.Model.Make.Name + " " + car.Model.Name,
                        PhotoUrl = carPhoto?.Url
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

        [HttpGet]
        public async Task DeletePhoto(string url)
        {
            var publicId = Path.ChangeExtension(url.Split("/").Last(), null);

            DeletionParams deletionParams = new DeletionParams(publicId);

            await this._cloudinary.DestroyAsync(deletionParams);
        }
    }
}
