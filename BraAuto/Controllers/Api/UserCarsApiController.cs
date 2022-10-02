using BraAuto.ViewModels;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/UserCars/[action]")]
    public class UserCarsApiController : BaseApiController
    {
        [HttpGet]
        public IActionResult Create(uint carId, uint userCarTypeId)
        {
            Db.UserCars.Insert(new UserCar
            {
                UserId = this.LoggedUser.Id,
                CarId = carId,
                UserCarTypeId = userCarTypeId
            });

            return this.Ok(new { });
        }

        public IActionResult Delete(uint carId, uint userCarTypeId)
        {
            Db.UserCars.Delete(this.LoggedUser.Id, carId, userCarTypeId);

            return this.Ok(new { });
        }
    }
}
