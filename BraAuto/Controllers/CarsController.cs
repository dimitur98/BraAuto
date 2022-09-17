using BraAuto.Helpers.Extensions;
using BraAuto.Resources;
using BraAuto.ViewModels;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;
using System.Drawing;
using BraAuto.ViewModels.Helpers;

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
            model.IsApproved = true;

            this.ExecuteSearch(model);

            return this.View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            var model = new CarCreateModel();

            this.LoadCarModel(model);

            model.ProductionDate = new DateTime(2000, 1,1);

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CarCreateModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var car = new Car
                    {
                        Description = model.Description,
                        Vin = model.Vin,
                        VehicleTypeId = model.VehicleTypeId,
                        ConditionId = model.ConditionId,
                        ModelId = model.ModelId,
                        Variant = model.Variant,
                        BodyTypeId = model.BodyTypeId,
                        ColorId = model.ColorId,
                        FuelTypeId = model.FuelTypeId,
                        ProductionDate = model.ProductionDate,
                        HorsePower = model.HorsePower,
                        CC = model.CC,
                        EuroStandartId = model.EuroStandartId,
                        GearboxTypeId = model.GearboxTypeId,
                        Price = model.Price,
                        LocationId = model.LocationId,
                        SpecificLocation = model.SpecificLocation,
                        Mileage = model.Mileage,
                        DoorNumberId = model.DoorNumberId,
                        HasAirConditioning = model.HasAirConditioning,
                        HasClimatronic = model.HasClimatronic,
                        HasLetherInterior = model.HasLetherInterior,
                        HasElectricWindows = model.HasElectricWindows,
                        HasElectricMirrors = model.HasElectricMirrors,
                        HasElectricSeats = model.HasElectricSeats,
                        HasSeatHeating = model.HasSeatHeating,
                        HasSunroof = model.HasSunroof,
                        HasStereo = model.HasStereo,
                        HasAlloyWheels = model.HasAlloyWheels,
                        HasDvdTv = model.HasDvdTv,
                        HasMultiSteeringWheel = model.HasMultiSteeringWheel,
                        HasAllWheelDrive = model.HasAllWheelDrive,
                        HasAbs = model.HasAbs,
                        HasEsp = model.HasEsp,
                        HasAirBag = model.HasAirBag,
                        HasXenonLights = model.HasXenonLights,
                        HasHalogenHeadlights = model.HasHalogenHeadlights,
                        HasTractionControl = model.HasTractionControl,
                        HasParktronic = model.HasParktronic,
                        HasAlarm = model.HasAlarm,
                        HasImmobilizer = model.HasImmobilizer,
                        HasCentralLock = model.HasCentralLock,
                        HasInsurance = model.HasInsurance,
                        IsArmored = model.IsArmored,
                        IsKeyless = model.IsKeyless,
                        IsTiptronicMultitronic = model.IsTiptronicMultitronic,
                        HasAutopilot = model.HasAutopilot,
                        HasPowerSteering = model.HasPowerSteering,
                        HasOnboardComputer = model.HasOnboardComputer,
                        HasServiceBook = model.HasServiceBook,
                        HasWarranty = model.HasWarranty,
                        HasNavigationSystem = model.HasNavigationSystem,
                        IsRightHandDrive = model.IsRightHandDrive,
                        HasTuning = model.HasTuning,
                        HasPanoramicRoof = model.HasPanoramicRoof,
                        IsTaxi = model.IsTaxi,
                        IsRetro = model.IsRetro,
                        HasTow = model.HasTow,
                        HasMoreSeats = model.HasMoreSeats,
                        HasRefrigerator = model.HasRefrigerator,
                        IsApproved = false,
                        IsAdvert = true,
                        CreatorId = this.LoggedUser.Id,
                        EditorId = this.LoggedUser.Id
                    };

                    Db.Cars.Insert(car);

                    return this.RedirectToAction(actionName: nameof(Home));
                }
            }
            catch (Exception ex)
            {
                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            this.LoadCarModel(model);

            return this.View(model);
        }

        [Authorize]
        public IActionResult Edit(uint id)
        {
            var car = Db.Cars.GetById(id);

            if (car == null) { return this.NotFound(); }

            car.LoadModel();

            var model = new CarEditModel
            {
                Id = car.Id,
                Description = car.Description,
                Vin = car.Vin,
                VehicleTypeId = car.VehicleTypeId,
                ConditionId = car.ConditionId,
                MakeId = car.Model.MakeId,
                ModelId = car.ModelId,
                Variant = car.Variant,
                BodyTypeId = car.BodyTypeId,
                ColorId = car.ColorId,
                FuelTypeId = car.FuelTypeId,
                ProductionDate = car.ProductionDate,
                HorsePower = car.HorsePower,
                CC = car.CC,
                EuroStandartId = car.EuroStandartId,
                GearboxTypeId = car.GearboxTypeId,
                Price = car.Price,
                LocationId = car.LocationId,
                SpecificLocation = car.SpecificLocation,
                Mileage = car.Mileage,
                DoorNumberId = car.DoorNumberId,
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
                HasRefrigerator = car.HasRefrigerator
            };

            this.LoadCarModel(model);

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(CarEditModel model)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var car = Db.Cars.GetById(model.Id);

                    if (car == null) { return this.NotFound(); }

                    car.Description = model.Description;
                    car.Vin = model.Vin;
                    car.VehicleTypeId = model.VehicleTypeId;
                    car.ConditionId = model.ConditionId;
                    car.ModelId = model.ModelId;
                    car.Variant = model.Variant;
                    car.BodyTypeId = model.BodyTypeId;
                    car.ColorId = model.ColorId;
                    car.FuelTypeId = model.FuelTypeId;
                    car.ProductionDate = model.ProductionDate;
                    car.HorsePower = model.HorsePower;
                    car.CC = model.CC;
                    car.EuroStandartId = model.EuroStandartId;
                    car.GearboxTypeId = model.GearboxTypeId;
                    car.Price = model.Price;
                    car.LocationId = model.LocationId;
                    car.SpecificLocation = model.SpecificLocation;
                    car.Mileage = model.Mileage;
                    car.DoorNumberId = model.DoorNumberId;
                    car.HasAirConditioning = model.HasAirConditioning;
                    car.HasClimatronic = model.HasClimatronic;
                    car.HasLetherInterior = model.HasLetherInterior;
                    car.HasElectricWindows = model.HasElectricWindows;
                    car.HasElectricMirrors = model.HasElectricMirrors;
                    car.HasElectricSeats = model.HasElectricSeats;
                    car.HasSeatHeating = model.HasSeatHeating;
                    car.HasSunroof = model.HasSunroof;
                    car.HasStereo = model.HasStereo;
                    car.HasAlloyWheels = model.HasAlloyWheels;
                    car.HasDvdTv = model.HasDvdTv;
                    car.HasMultiSteeringWheel = model.HasMultiSteeringWheel;
                    car.HasAllWheelDrive = model.HasAllWheelDrive;
                    car.HasAbs = model.HasAbs;
                    car.HasEsp = model.HasEsp;
                    car.HasAirBag = model.HasAirBag;
                    car.HasXenonLights = model.HasXenonLights;
                    car.HasHalogenHeadlights = model.HasHalogenHeadlights;
                    car.HasTractionControl = model.HasTractionControl;
                    car.HasParktronic = model.HasParktronic;
                    car.HasAlarm = model.HasAlarm;
                    car.HasImmobilizer = model.HasImmobilizer;
                    car.HasCentralLock = model.HasCentralLock;
                    car.HasInsurance = model.HasInsurance;
                    car.IsArmored = model.IsArmored;
                    car.IsKeyless = model.IsKeyless;
                    car.IsTiptronicMultitronic = model.IsTiptronicMultitronic;
                    car.HasAutopilot = model.HasAutopilot;
                    car.HasPowerSteering = model.HasPowerSteering;
                    car.HasOnboardComputer = model.HasOnboardComputer;
                    car.HasServiceBook = model.HasServiceBook;
                    car.HasWarranty = model.HasWarranty;
                    car.HasNavigationSystem = model.HasNavigationSystem;
                    car.IsRightHandDrive = model.IsRightHandDrive;
                    car.HasTuning = model.HasTuning;
                    car.HasPanoramicRoof = model.HasPanoramicRoof;
                    car.IsTaxi = model.IsTaxi;
                    car.IsRetro = model.IsRetro;
                    car.HasTow = model.HasTow;
                    car.HasMoreSeats = model.HasMoreSeats;
                    car.HasRefrigerator = model.HasRefrigerator;

                    Db.Cars.Update(car);

                    return this.RedirectToAction(nameof(My));
                }
            }
            catch (Exception ex)
            {

                ex.SaveToLog();
                this.ModelState.AddModelError(string.Empty, Global.GeneralError);
            }

            this.LoadCarModel(model);

            return this.View(model);
        }

        public IActionResult Delete(uint id)
        {
            try
            {
                var car = Db.Cars.GetById(id);

                if (car == null) { return this.NotFound(); }

                if (!this.LoggedUser.IsAdmin() || this.LoggedUser.Id != car.CreatorId) { return this.RedirectToHttpForbidden(); }

                Db.Cars.Delete(id);

                this.TempData[Global.AlertKey] = new Alert(Global.ItemDeleted, AlertTypes.Info);
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
            var car = Db.Cars.GetById(id);

            if (car == null) { return this.NotFound(); }

            var carModel = Db.Models.GetById(car.ModelId);

            carModel.LoadMake();
            car.LoadCreator();

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
                OwnerName = car.Creator.Name,
                Mobile = car.Creator.Mobile,
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

        protected void LoadCarModel(CarBaseModel model)
        {
            model.VehicleTypes = Db.VehicleTypes.GetAll();
            model.BodyTypes = Db.BodyTypes.GetAll();
            model.Makes = Db.Makes.GetAll();
            model.Conditions = Db.Conditions.GetAll();
            model.Colors = Db.Colors.GetAll();
            model.FuelTypes = Db.FuelTypes.GetAll();
            model.GearboxTypes = Db.GearboxTypes.GetAll();
            model.EuroStandarts = Db.EuroStandarts.GetAll();
            model.Locations = Db.Locations.GetAll();
            model.DoorNumbers = Db.DoorNumbers.GetAll();
        }
    }
}
