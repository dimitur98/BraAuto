using BraAuto.Helpers.Extensions;
using BraAuto.Resources;
using BraAuto.ViewModels;
using BraAuto.ViewModels.Common;
using BraAuto.ViewModels.Helpers;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using BraAutoDb.Models.UserCarsSearch;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers
{
    public class UserCarsController : BaseController
    {
        public IActionResult MyService(MyUserCarServiceModel model)
        {
            if (this.LoggedUser.IsService())
            {
                model.UserId = this.LoggedUser.Id;
            }
            else
            {
                model.CreatorId = this.LoggedUser.Id;
            }

            model.UserCarTypeIds = Config.GetSection("UserCar.Service.Ids").Get<uint[]>();

            this.ExecuteSearch(model);

            return this.View(model);
        }

        public IActionResult Favourite(FavouriteCarModel model)
        {
            model.CreatorId = this.LoggedUser.Id;
            model.UserCarTypeIds = new uint[] { Db.UserCarTypes.FavouriteId };

            this.ExecuteSearch(model);

            return this.View(model);
        }

        public IActionResult BookAppointment(uint? carId)
        {
            var model = new UserServiceBookAppointmentModel();

            this.LoadBookAppoitmentModel(model);

            model.Date = DateTime.Now;

            if (carId != null) { model.CarId = carId.Value; }

            return this.View(model);
        }

        [HttpPost]
        public IActionResult BookAppointment(UserServiceBookAppointmentModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var service = Db.Users.GetById(model.ServiceId);
                    var serviceTotalBookings = Db.UserCars.Search(new Request { CarIds = new uint[] { model.CarId }, UserCarTypeIds = new uint[] { Db.UserCarTypes.ServiceAppointmentId, Db.UserCarTypes.ServiceAppointmentApprovedId }, Date = model.Date }).Records.Count();

                    if (service.MaxBookingsPerDay <= serviceTotalBookings)
                    {
                        this.LoadBookAppoitmentModel(model);
                        this.TempData[Global.AlertKey] = new Alert(Global.MaxBookingAppointmentsReached, AlertTypes.Danger).SerializeAlert();

                        return this.View(model);
                    }

                    var freeHours = service.GetFreeHours(model.Date);

                    if (!freeHours.Contains(model.Hour))
                    {
                        this.LoadBookAppoitmentModel(model);
                        this.TempData[Global.AlertKey] = new Alert(Global.SelectedTimeNotFree, AlertTypes.Danger).SerializeAlert();

                        return this.View(model);
                    }

                    var time = new TimeSpan((int)model.Hour, 0, 0);

                    var appointment = new UserCar
                    {
                        UserId = service.Id,
                        CarId = model.CarId,
                        UserCarTypeId = Db.UserCarTypes.ServiceAppointmentId,
                        Date = model.Date.Add(time),
                        Description = model.Description,
                        CreatorId = this.LoggedUser.Id
                    };

                    Db.UserCars.Insert(appointment);

                    return this.RedirectToAction(nameof(MyService));
                }
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            this.LoadBookAppoitmentModel(model);

            return this.View(model);
        }

        public IActionResult Delete(uint id)
        {
            var userCar = Db.UserCars.GetById(id);

            if(userCar == null) { return this.NotFound(); }

            userCar.LoadUser();
            
            if (!userCar.User.IsService()) { return this.RedirectToHttpForbidden(); }

            Db.UserCars.Delete(id);

            return RedirectToAction(nameof(MyService));
        }

        protected void ExecuteSearch(UserCarSearchBaseModel model)
        {
            model.SetDefaultSort("uc.id", sortDesc: true);

            var response = Db.UserCars.Search(model.ToSearchRequest());

            Db.UserCars.LoadCars(response.Records);
            Db.Cars.LoadModels(response.Records.Select(r => r.Car));
            Db.Models.LoadMakes(response.Records.Select(r => r.Car.Model));
            Db.Cars.LoadPhotoUrls(response.Records.Select(r => r.Car));

            model.Response = response;
        }

        protected void LoadBookAppoitmentModel(UserServiceBookAppointmentModel model)
        {
            var cars = Db.Cars.GetByUserId(this.LoggedUser.Id);
            cars.AddRange(Db.Cars.GetAll(isApproved: true, isAdvert: true));

            Db.Cars.LoadModels(cars);
            Db.Models.LoadMakes(cars.Select(c => c.Model));

            model.Cars = cars.Select(c => new SimpleModel(c.Id, $"{c.Model.Make.Name} {c.Model.Name} {c.Variant} {(c.CreatorId == this.LoggedUser.Id ? "(My)" : "")}"));
            model.Services = Db.Users.GetByTypeId(Db.UserTypes.ServiceId).Select(u => new SimpleModel(u.Id, u.Name));
        }
    }
}