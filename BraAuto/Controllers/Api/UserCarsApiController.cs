﻿using BraAuto.ViewModels;
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
                CarId = carId,
                UserCarTypeId = userCarTypeId,
                CreatorId = this.LoggedUser.Id
            });

            return this.Ok(new { });
        }

        [HttpGet]
        public IActionResult Edit(uint id, uint userCarTypeId)
        {
            var userCar = Db.UserCars.GetById(id);

            if (userCar == null) { return this.NotFound(); }

            userCar.UserCarTypeId = userCarTypeId;

            Db.UserCars.Update(userCar);

            return this.Ok(new { });
        }

        public IActionResult Delete(uint carId, uint userCarTypeId)
        {
            Db.UserCars.Delete(carId, userCarTypeId, this.LoggedUser.Id);

            return this.Ok(new { });
        }
    }
}
