using BraAuto.Helpers.Extensions;
using BraAuto.Resources;
using BraAuto.ViewModels;
using BraAuto.ViewModels.Helpers;
using BraAutoDb.Dal;
using BraAutoDb.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BraAuto.Controllers
{
    public class CarsController : BaseController
    {
        private Cloudinary _cloudinary;

        public CarsController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        [AllowAnonymous]
        public IActionResult Home()
        {
            var model = new HomeCarSearchModel
            {
                Makes = Db.Makes.GetAll(),
                Years = Db.Years.GetAll(),
                NewestCars = Db.Cars.Search(new BraAutoDb.Models.CarsSearch.Request { IsActive = true, IsAdvert = true, SortColumn = "created_at", RowCount = 10 }).Records,
                MostViewedCars = Db.Cars.GetMostViewed(10, true),
                NewestArticles = Db.Articles.Search(new BraAutoDb.Models.ArticlesSearch.Request { IsActive = true, SortColumn = "created_at", RowCount = 6 }).Records
            };

            this.LoadCarProperties(model.NewestCars);
            this.LoadCarProperties(model.MostViewedCars);

            Db.Articles.LoadCreators(model.NewestArticles);

            return this.View(model);
        }

        [Authorize]
        public IActionResult My(MyCarModel model)
        {
            model.UserIds = new List<uint>() { this.LoggedUser.Id };
            model.FavouriteCount = Db.UserCars.GetCount(Db.UserCarTypes.FavouriteId);

            this.ExecuteSearch(model);

            return this.View(model);
        }

        public IActionResult Favourite(FavouriteCarModel model)
        {
            model.GetFavouriteCarsOnly = true;
            model.UserIds = new uint[] { this.LoggedUser.Id };

            this.ExecuteSearch(model);

            return this.View(model);
        }

        [Authorize(Roles = "administrator")]
        public IActionResult Admin(CarAdminModel model)
        {
            model.FavouriteCount = Db.UserCars.GetCount(Db.UserCarTypes.FavouriteId);

            this.ExecuteSearch(model);

            Db.Cars.LoadCreators(model.Response.Records);

            return this.View(model);
        }

        [AllowAnonymous]
        public IActionResult Search(CarSearchModel model)
        {
            model.VehicleTypes = Db.VehicleTypes.GetAll();
            model.Makes = Db.Makes.GetAll();
            model.Years = Db.Years.GetAll();
            model.IsApproved = true;
            model.ShowSpecificSortFields = false;

            this.ExecuteSearch(model);

            if (this.LoggedUser != null)
            {
                model.UserFavourableCarIds = Db.UserCars.GetByUserId(this.LoggedUser.Id, Db.UserCarTypes.FavouriteId).Select(ur => ur.CarId);
                model.UserCompareCarIds = Db.UserCars.GetByUserId(this.LoggedUser.Id, Db.UserCarTypes.CompareId).Select(ur => ur.CarId);
            }

            return this.View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            var model = new CarCreateModel();

            this.LoadCarModel(model);

            model.ProductionDate = new DateTime(2000, 1, 1);

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CarCreateModel model)
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

                    await this.UploadPhotos(model.Photos, car.Id);

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

            model.Photos.LoadPhotoUrls(car.Id);

            this.LoadCarModel(model);

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(CarEditModel model)
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

                    Db.CarPhotos.DeleteByCarId(car.Id);
                    await this.UploadPhotos(model.Photos, car.Id);

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

        public async Task<IActionResult> Delete(uint id)
        {
            try
            {
                var car = Db.Cars.GetById(id);

                if (car == null) { return this.NotFound(); }

                if (!this.LoggedUser.IsAdmin() || this.LoggedUser.Id != car.CreatorId) { return this.RedirectToHttpForbidden(); }

                Db.Cars.Delete(car.Id);

                await this.DeletePhotos(Db.CarPhotos.GetByCarId(car.Id).Select(ci => ci.Url));

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

            var model = new CarDetailsModel
            {
                Id = car.Id,
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
                PhotoUrls = Db.CarPhotos.GetByCarId(car.Id).Select(ci => ci.Url),
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

            var ip = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();

            if (Db.CarViews.Get(car.Id, ip) == null)
            {
                Db.CarViews.Insert(new CarView { CarId = car.Id, UserIp = ip });
            }

            model.ViewsCount = Db.CarViews.GetCountByCarId(car.Id);

            if (this.LoggedUser != null)
            {
                model.IsFavourite = Db.UserCars.Get(new uint[] { Db.UserCarTypes.FavouriteId },carId: car.Id, userId: this.LoggedUser.Id).FirstOrDefault() != null;
            }

            return this.View(model);
        }

        public IActionResult Compare()
        {
            var carIds = Db.UserCars.GetByUserId(this.LoggedUser.Id, userCarTypeId: Db.UserCarTypes.CompareId)?.Select(uc => uc.CarId);
            var cars = Db.Cars.GetByIds(carIds);

            if (cars.IsNullOrEmpty()) { return this.NotFound(); }

            var model = new CarCompareModel
            {
                Cars = new List<CarSpecifications>()
            };

            foreach (var car in cars)
            {
                var carModel = Db.Models.GetById(car.ModelId);

                carModel.LoadMake();
                car.LoadCreator();

                model.Cars.Add(new CarSpecifications
                {
                    Id = car.Id,
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
                    PhotoUrls = Db.CarPhotos.GetByCarId(car.Id).Select(ci => ci.Url),
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
                });
            }

            return this.View(model);
        }

        protected void ExecuteSearch(CarSearchBaseModel model)
        {
            model.SetDefaultSort("c.created_at", sortDesc: true);

            var request = model.ToSearchRequest();

            request.IsAdvert = true;
            request.ReturnTotalRecords = true;

            var response = Db.Cars.Search(request);

            LoadCarProperties(response.Records);

            model.Response = response;
        }

        protected void LoadCarProperties(IEnumerable<Car> cars)
        {
            Db.Cars.LoadModels(cars);
            Db.Models.LoadMakes(cars.Select(r => r.Model));
            Db.Cars.LoadGearboxTypes(cars);
            Db.Cars.LoadPhotoUrls(cars);
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

        protected async Task UploadPhotos(Photos photos, uint carId)
        {
            var photoProps = photos.GetType().GetProperties().Where(p => p.PropertyType == typeof(IFormFile)).ToList();
            var urlProps = photos.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)).ToList();
            var urlsForDelete = new List<string>();
            var index = 1;

            for (int i = 0; i < photoProps.Count(); i++)
            {
                var photo = (IFormFile)photoProps[i].GetValue(photos);
                var oldUrl = (string)urlProps[i].GetValue(photos);
                var url = photo.IsValidPhoto() ? await photo.UploadPhotoAsync() : oldUrl;

                if (!string.IsNullOrEmpty(url))
                {
                    Db.CarPhotos.Insert(new CarPhoto
                    {
                        Url = url,
                        CarId = carId,
                        SortOrder = index
                    });

                    index++;
                }

                if (photo.IsValidPhoto() && !string.IsNullOrEmpty(oldUrl)) { urlsForDelete.Add(oldUrl); }
            }

            if (!urlsForDelete.IsNullOrEmpty()) { await this.DeletePhotos(urlsForDelete); }
        }

        protected async Task DeletePhotos(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                var publicId = System.IO.Path.ChangeExtension(url.Split("/").Last(), null);

                DeletionParams deletionParams = new DeletionParams(publicId);

                await this._cloudinary.DestroyAsync(deletionParams);
            }
        }
    }
}
