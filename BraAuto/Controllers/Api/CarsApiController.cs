using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers.Api
{
    [Route("/api/Cars/[action]")]
    public class CarsApiController : BaseApiController
    {
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
