using BraAuto.Helpers.Extensions;
using BraAuto.ViewModels;
using BraAutoDb.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers
{
    public class CarsController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Home()
        {
            var model = new HomeCarSearchModel
            {
                Makes = Db.Makes.GetAll(),
                Years = Db.Years.GetAll()
        }   ;

            return this.View(model);
        }

        [Authorize]
        public IActionResult My(MyCarModel model)
        {
            model.UserIds = new List<uint>() { this.LoggedUser.Id };

            this.ExecuteSearch(model);

            return this.View(model);
        }

        [AllowAnonymous]
        public IActionResult Search(CarSearchModel model)
        {
            model.VehicleTypes = Db.VehicleTypes.GetAll();
            model.Makes = Db.Makes.GetAll();
            model.Years = Db.Years.GetAll();

            this.ExecuteSearch(model);

            return this.View(model);
        }

        public IActionResult Details(uint id)
        {
            var car = Db.Cars.GetById(id);

            if (car == null) { return this.NotFound(); }

            var owner = Db.Users.GetById(car.CreatorId);
            var carModel = Db.Models.GetById(car.ModelId);
            carModel.LoadMake();

            var model = new DetailsCarModel
            {
                Description = car.Description,
                VehicleType = Db.VehicleTypes.GetById(car.VehicleTypeId).Name,
                Condition = Db.Conditions.GetById(car.ConditionId).Name,
                Make = carModel.Make.Name,
                Model = carModel.Name,
                Variant = car.Variant,
                BodyType = Db.BodyTypes.GetById(car.BodyTypeId).Name,
                Color = Db.Colors.GetById(car.ColorId).Name,
                FuelType = Db.FuelTypes.GetById(car.FuelTypeId).Name,
                ProductionDate = car.ProductionDate.ToWebDateFormat(showDay: false).ToString(),
                HorsePower = car.HorsePower.ToString(),
                Cc = car.CC.ToString(),
                EuroStandart = Db.EuroStandarts.GetById(car.EuroStandartId).Name,
                GearboxType = Db.GearboxTypes.GetById(car.GearboxTypeId).Name,
                Price = car.Price.ToString(),
                Location = Db.Locations.GetById(car.LocationId).Name,
                SpecificLocation = car.SpecificLocation,
                Mileage = car.Mileage.ToString(),
                DoorNumber = Db.DoorNumbers.GetById(car.DoorNumberId).Name,
                OwnerName = owner.Name,
                Mobile = owner.Mobile,
                HasAirConditioning = car.HasAirConditioning,
                HasClimatronic = car.HasClimatronic,
                HasLetherInterior = car.HasLetherInterior,
                HasElectricWindows = car.HasElectricWindows,
                HasElectricMirrors = car.HasElectricMirrors,
                HasElectricSeats = car.HasElectricSeats,
                HasSeatHeating = car.HasSeatHeating,
                HasSunroof = car.HasSunroof,
                HasStereo = car.HasStereo,
                HasAlloyWheels = car.HasAlloyWheels,
                HasDvdTv = car.HasDvdTv,
                HasMultiSteeringWheel = car.HasMultiSteeringWheel,
                HasAllWheelDrive = car.HasAllWheelDrive,
                HasAbs = car.HasAbs,
                HasEsp = car.HasEsp,
                HasAirBag = car.HasAirBag,
                HasXenonLights = car.HasXenonLights,
                HasHalogenHeadlights = car.HasHalogenHeadlights,
                HasTractionControl = car.HasTractionControl,
                HasParktronic = car.HasParktronic,
                HasAlarm = car.HasAlarm,
                HasImmobilizer = car.HasImmobilizer,
                HasCentralLock = car.HasCentralLock,
                HasInsurance = car.HasInsurance,
                IsArmored = car.IsArmored,
                IsKeyless = car.IsKeyless,
                IsTiptronicMultitronic = car.IsTiptronicMultitronic,
                HasAutopilot = car.HasAutopilot,
                HasPowerSteering = car.HasPowerSteering,
                HasOnboardComputer = car.HasOnboardComputer,
                HasServiceBook = car.HasServiceBook,
                HasWarranty = car.HasWarranty,
                HasNavigationSystem = car.HasNavigationSystem,
                IsRightHandDrive = car.IsRightHandDrive,
                HasTuning = car.HasTuning,
                HasPanoramicRoof = car.HasPanoramicRoof,
                IsTaxi = car.IsTaxi,
                IsRetro = car.IsRetro,
                HasTow = car.HasTow,
                HasMoreSeats = car.HasMoreSeats,
                HasRefrigerator = car.HasRefrigerator,
            };

            return this.View(model);
        }

        protected void ExecuteSearch(CarSearchModel model)
        {
            model.SetDefaultSort("c.created_at", sortDesc: true);

            var request = model.ToSearchRequest();

            request.IsAdvert = true;
            request.ReturnTotalRecords = true;

            var response = Db.Cars.Search(request);

            Db.Cars.LoadModels(response.Records);
            Db.Models.LoadMakes(response.Records.Select(r => r.Model));
            Db.Cars.LoadGearboxTypes(response.Records);

            model.Response = response;
        }
    }
}
