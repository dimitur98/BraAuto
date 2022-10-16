using BraAutoDb.Dal;
using BraAutoDb.Models;
using BraAutoDb.Models.CarsSearch;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BraAuto.ViewModels
{
    public abstract class CarSearchBaseModel : BaseSearchModel<Response, Car>
    {
        public CarSearchBaseModel()
        {
            this.SortFields = new List<(string Name, string SortColumn, bool SortDesc, bool Specific)> { ("Newest First", "c.created_at", true, false), ("Oldest First", "c.created_at", false, false), ("Price High-Low", "c.price", true, false), ("Price Low-High", "c.price", false, false), ("Approved First", "c.is_approved", true, true), ("Not Approved First", "c.is_approved", false, true) };
        }

        public IEnumerable<uint> Ids { get; set; }

        public IEnumerable<uint> UserCarTypeIds { get; set; }

        [DisplayName("Vehicle Type")]
        public uint? VehicleTypeId { get; set; }
        public IEnumerable<VehicleType> VehicleTypes { get; set; }

        [DisplayName("Body Type")]
        public IEnumerable<uint> BodyTypeIds { get; set; }

        [DisplayName("Make")]
        public uint? MakeId { get; set; }
        public IEnumerable<Make> Makes { get; set; }

        [DisplayName("Model")]
        public IEnumerable<uint> ModelIds { get; set; }

        [DisplayName("Condition")]
        public IEnumerable<uint> ConditionIds { get; set; }

        [DisplayName("Year")]
        public uint? YearFromId { get; set; }

        [DisplayName("To")]
        public uint? YearToId { get; set; }
        public IEnumerable<Year> Years { get; set; }

        [DisplayName("Color")]
        public IEnumerable<uint> ColorIds { get; set; }

        [DisplayName("Fuel")]
        public IEnumerable<uint> FuelTypeIds { get; set; }

        [DisplayName("Gearbox")]
        public IEnumerable<uint> GearboxTypeIds { get; set; }

        [DisplayName("Horse Power")]
        public uint? HorsePowerFrom { get; set; }

        [DisplayName("Euro Standart")]
        public IEnumerable<uint> EuroStandartIds { get; set; }

        [DisplayName("Price")]
        public uint? PriceFrom { get; set; }

        public uint? PriceTo { get; set; }

        [DisplayName("Location")]
        public IEnumerable<uint> LocationIds { get; set; }

        [DisplayName("Door Number")]
        public IEnumerable<uint> DoorNumberIds { get; set; }

        [DisplayName("User")]
        public IEnumerable<uint> UserIds { get; set; }

        public bool GetFavouriteCarsOnly { get; set; } = false;

        [DisplayName("Air Conditioning")]
        public bool? HasAirConditioning { get; set; }

        [DisplayName("Climatronic")]
        public bool? HasClimatronic { get; set; }

        [DisplayName("Lether Interior")]
        public bool? HasLetherInterior { get; set; }

        [DisplayName("Electric Windows")]
        public bool? HasElectricWindows { get; set; }

        [DisplayName("Electric Mirrors")]
        public bool? HasElectricMirrors { get; set; }

        [DisplayName("Electric Seats")]
        public bool? HasElectricSeats { get; set; }

        [DisplayName("Seat Heating")]
        public bool? HasSeatHeating { get; set; }

        [DisplayName("Sunroof")]
        public bool? HasSunroof { get; set; }

        [DisplayName("Stereo")]
        public bool? HasStereo { get; set; }

        [DisplayName("Alloy Wheels")]
        public bool? HasAlloyWheels { get; set; }

        [DisplayName("Dvd/Tv")]
        public bool? HasDvdTv { get; set; }

        [DisplayName("Multi Steering Wheel")]
        public bool? HasMultiSteeringWheel { get; set; }

        [DisplayName("4WD")]
        public bool? HasAllWheelDrive { get; set; }

        [DisplayName("ABS")]
        public bool? HasAbs { get; set; }

        [DisplayName("ESP")]
        public bool? HasEsp { get; set; }

        [DisplayName("AirBag")]
        public bool? HasAirBag { get; set; }

        [DisplayName("Xenon Lights")]
        public bool? HasXenonLights { get; set; }

        [DisplayName("Halogen Lights")]
        public bool? HasHalogenHeadlights { get; set; }

        [DisplayName("Traction Control")]
        public bool? HasTractionControl { get; set; }

        [DisplayName("Parktronic")]
        public bool? HasParktronic { get; set; }

        [DisplayName("Alarm")]
        public bool? HasAlarm { get; set; }

        [DisplayName("Imobilizer")]
        public bool? HasImmobilizer { get; set; }

        [DisplayName("Central Lock")]
        public bool? HasCentralLock { get; set; }

        [DisplayName("Insurance")]
        public bool? HasInsurance { get; set; }

        [DisplayName("Armored")]
        public bool? IsArmored { get; set; }

        [DisplayName("Keyless")]
        public bool? IsKeyless { get; set; }

        [DisplayName("Tiptronic/Multitronic")]
        public bool? IsTiptronicMultitronic { get; set; }

        [DisplayName("Autopilot")]
        public bool? HasAutopilot { get; set; }

        [DisplayName("Power Steering")]
        public bool? HasPowerSteering { get; set; }

        [DisplayName("Onboard Computer")]
        public bool? HasOnboardComputer { get; set; }

        [DisplayName("Service Book")]
        public bool? HasServiceBook { get; set; }

        [DisplayName("Warranty")]
        public bool? HasWarranty { get; set; }

        [DisplayName("Navigation System")]
        public bool? HasNavigationSystem { get; set; }

        [DisplayName("Right Hand Drive")]
        public bool? IsRightHandDrive { get; set; }

        [DisplayName("Tuning")]
        public bool? HasTuning { get; set; }

        [DisplayName("Panoramic Roof")]
        public bool? HasPanoramicRoof { get; set; }

        [DisplayName("Taxi")]
        public bool? IsTaxi { get; set; }

        [DisplayName("Retro")]
        public bool? IsRetro { get; set; }

        [DisplayName("Tow")]
        public bool? HasTow { get; set; }

        [DisplayName("More Seats")]
        public bool? HasMoreSeats { get; set; }

        [DisplayName("Refrigerator")]
        public bool? HasRefrigerator { get; set; }

        public bool? IsApproved { get; set; }

        public IEnumerable<(uint CarId, int Count)> FavouriteCount { get; set; }

        public Request ToSearchRequest()
        {
            var request = new Request
            {
                Ids = this.Ids,
                UserCarTypeIds = this.UserCarTypeIds,
                VehicleTypeId = this.VehicleTypeId,
                BodyTypeIds = this.BodyTypeIds,
                MakeId = this.MakeId,
                ModelIds = this.ModelIds,
                ConditionIds = this.ConditionIds,
                YearFromId = this.YearFromId,
                YearToId = this.YearToId,
                ColorIds = this.ColorIds,
                FuelTypeIds = this.FuelTypeIds,
                GearboxTypeIds = this.GearboxTypeIds,
                HorsePowerFrom = this.HorsePowerFrom,
                EuroStandartIds = this.EuroStandartIds,
                PriceFrom = this.PriceFrom,
                PriceTo = this.PriceTo,
                LocationIds = this.LocationIds,
                DoorNumberIds = this.DoorNumberIds,
                HasAirConditioning = this.HasAirConditioning,
                UserIds = this.UserIds,
                GetFavouriteCarsOnly = this.GetFavouriteCarsOnly,
                HasClimatronic = this.HasClimatronic,
                HasLetherInterior = this.HasLetherInterior,
                HasElectricWindows = this.HasElectricWindows,
                HasElectricMirrors = this.HasElectricMirrors,
                HasElectricSeats = this.HasElectricSeats,
                HasSeatHeating = this.HasSeatHeating,
                HasSunroof = this.HasSunroof,
                HasStereo = this.HasStereo,
                HasAlloyWheels = this.HasAlloyWheels,
                HasDvdTv = this.HasDvdTv,
                HasMultiSteeringWheel = this.HasMultiSteeringWheel,
                HasAllWheelDrive = this.HasAllWheelDrive,
                HasAbs = this.HasAbs,
                HasEsp = this.HasEsp,
                HasAirBag = this.HasAirBag,
                HasXenonLights = this.HasXenonLights,
                HasHalogenHeadlights = this.HasHalogenHeadlights,
                HasTractionControl = this.HasTractionControl,
                HasParktronic = this.HasParktronic,
                HasAlarm = this.HasAlarm,
                HasImmobilizer = this.HasImmobilizer,
                HasCentralLock = this.HasCentralLock,
                HasInsurance = this.HasInsurance,
                IsArmored = this.IsArmored,
                IsKeyless = this.IsKeyless,
                IsTiptronicMultitronic = this.IsTiptronicMultitronic,
                HasAutopilot = this.HasAutopilot,
                HasPowerSteering = this.HasPowerSteering,
                HasOnboardComputer = this.HasOnboardComputer,
                HasServiceBook = this.HasServiceBook,
                HasWarranty = this.HasWarranty,
                HasNavigationSystem = this.HasNavigationSystem,
                IsRightHandDrive = this.IsRightHandDrive,
                HasTuning = this.HasTuning,
                HasPanoramicRoof = this.HasPanoramicRoof,
                IsTaxi = this.IsTaxi,
                IsRetro = this.IsRetro,
                HasTow = this.HasTow,
                HasMoreSeats = this.HasMoreSeats,
                HasRefrigerator = this.HasRefrigerator,
                IsApproved = this.IsApproved
            };

            this.SetSearchRequest(request);

            return request;
        }
    }

    public class CarSearchModel : CarSearchBaseModel
    {
        public IEnumerable<uint> UserFavourableCarIds { get; set; }

        public IEnumerable<uint> UserCompareCarIds { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("Search", "Cars") };

            return new Breadcrumb(paths, totalRecords: this.Response.TotalRecords);
        }
    }
    public class HomeCarSearchModel : CarSearchBaseModel 
    {
        public IEnumerable<Car> NewestCars { get; set; }

        public IEnumerable<Car> MostViewedCars { get; set; }

        public IEnumerable<Article> NewestArticles { get; set; }
    }

    public class MyCarModel : CarSearchBaseModel
    { 
        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("My", "Cars") };

            return new Breadcrumb(paths, totalRecords: this.Response.TotalRecords);
        }
    }

    public class CarAdminModel : CarSearchBaseModel
    {
        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("Admin", "Cars") };

            return new Breadcrumb(paths, totalRecords: this.Response.TotalRecords);
        }
    }

    public class CarBaseModel
    {
        [Required]
        [DisplayName("Vehicle Type")]
        public uint VehicleTypeId { get; set; }
        public IEnumerable<VehicleType> VehicleTypes { get; set; }

        [Required]
        [DisplayName("Body Type")]
        public uint BodyTypeId { get; set; }
        public IEnumerable<BodyType> BodyTypes { get; set; }

        [Required]
        [DisplayName("Make")]
        public uint? MakeId { get; set; }
        public IEnumerable<Make> Makes { get; set; }

        [Required]
        [DisplayName("Model")]
        public uint ModelId { get; set; }

        [DisplayName("Variant")]
        public string Variant { get; set; }

        [Required]
        [DisplayName("Condition")]
        public uint ConditionId { get; set; }
        public IEnumerable<Condition> Conditions { get; set; }

        [Required]
        [DisplayName("Production Date")]
        public DateTime ProductionDate { get; set; }

        [DisplayName("Color")]
        public uint ColorId { get; set; }
        public IEnumerable<Color> Colors { get; set; }

        [Required]
        [DisplayName("Fuel")]
        public uint FuelTypeId { get; set; }
        public IEnumerable<FuelType> FuelTypes { get; set; }

        [Required]
        [DisplayName("Gearbox")]
        public uint GearboxTypeId { get; set; }
        public IEnumerable<GearboxType> GearboxTypes { get; set; }

        [DisplayName("Horse Power")]
        public uint? HorsePower { get; set; }

        [DisplayName("CC")]
        public uint? CC { get; set; }

        [DisplayName("Euro Standart")]
        public uint EuroStandartId { get; set; }
        public IEnumerable<EuroStandart> EuroStandarts { get; set; }

        [Required]
        [DisplayName("Price")]
        public uint Price { get; set; }

        [Required]
        [DisplayName("Location")]
        public uint LocationId { get; set; }
        public IEnumerable<Location> Locations { get; set; }

        [DisplayName("Specific Location")]
        public string SpecificLocation { get; set; }

        [DisplayName("Mileage")]
        public uint Mileage { get; set; }

        [DisplayName("Door Number")]
        public uint DoorNumberId { get; set; }
        public IEnumerable<DoorNumber> DoorNumbers { get; set; }

        [DisplayName("Vin")]
        public string Vin { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public Photos Photos { get; set; }

        [DisplayName("Air Conditioning")]
        public bool HasAirConditioning { get; set; }

        [DisplayName("Climatronic")]
        public bool HasClimatronic { get; set; }

        [DisplayName("Lether Interior")]
        public bool HasLetherInterior { get; set; }

        [DisplayName("Electric Windows")]
        public bool HasElectricWindows { get; set; }

        [DisplayName("Electric Mirrors")]
        public bool HasElectricMirrors { get; set; }

        [DisplayName("Electric Seats")]
        public bool HasElectricSeats { get; set; }

        [DisplayName("Seat Heating")]
        public bool HasSeatHeating { get; set; }

        [DisplayName("Sunroof")]
        public bool HasSunroof { get; set; }

        [DisplayName("Stereo")]
        public bool HasStereo { get; set; }

        [DisplayName("Alloy Wheels")]
        public bool HasAlloyWheels { get; set; }

        [DisplayName("Dvd/Tv")]
        public bool HasDvdTv { get; set; }

        [DisplayName("Multi Steering Wheel")]
        public bool HasMultiSteeringWheel { get; set; }

        [DisplayName("4WD")]
        public bool HasAllWheelDrive { get; set; }

        [DisplayName("ABS")]
        public bool HasAbs { get; set; }

        [DisplayName("ESP")]
        public bool HasEsp { get; set; }

        [DisplayName("AirBag")]
        public bool HasAirBag { get; set; }

        [DisplayName("Xenon Lights")]
        public bool HasXenonLights { get; set; }

        [DisplayName("Halogen Lights")]
        public bool HasHalogenHeadlights { get; set; }

        [DisplayName("Traction Control")]
        public bool HasTractionControl { get; set; }

        [DisplayName("Parktronic")]
        public bool HasParktronic { get; set; }

        [DisplayName("Alarm")]
        public bool HasAlarm { get; set; }

        [DisplayName("Imobilizer")]
        public bool HasImmobilizer { get; set; }

        [DisplayName("Central Lock")]
        public bool HasCentralLock { get; set; }

        [DisplayName("Insurance")]
        public bool HasInsurance { get; set; }

        [DisplayName("Armored")]
        public bool IsArmored { get; set; }

        [DisplayName("Keyless")]
        public bool IsKeyless { get; set; }

        [DisplayName("Tiptronic/ Multitronic")]
        public bool IsTiptronicMultitronic { get; set; }

        [DisplayName("Autopilot")]
        public bool HasAutopilot { get; set; }

        [DisplayName("Power Steering")]
        public bool HasPowerSteering { get; set; }

        [DisplayName("Onboard Computer")]
        public bool HasOnboardComputer { get; set; }

        [DisplayName("Service Book")]
        public bool HasServiceBook { get; set; }

        [DisplayName("Warranty")]
        public bool HasWarranty { get; set; }

        [DisplayName("Navigation System")]
        public bool HasNavigationSystem { get; set; }

        [DisplayName("Right Hand Drive")]
        public bool IsRightHandDrive { get; set; }

        [DisplayName("Tuning")]
        public bool HasTuning { get; set; }

        [DisplayName("Panoramic Roof")]
        public bool HasPanoramicRoof { get; set; }

        [DisplayName("Taxi")]
        public bool IsTaxi { get; set; }

        [DisplayName("Retro")]
        public bool IsRetro { get; set; }

        [DisplayName("Tow")]
        public bool HasTow { get; set; }

        [DisplayName("More Seats")]
        public bool HasMoreSeats { get; set; }

        [DisplayName("Refrigerator")]
        public bool HasRefrigerator { get; set; }

        [DisplayName("Is Advert")]
        public bool IsAdvert { get; set; } = true;
    }

    public class CarCreateModel : CarBaseModel
    {
        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("Create", "Cars") };

            return new Breadcrumb(paths);
        }
    }

    public class CarEditModel : CarBaseModel
    {
        public CarEditModel()
        {
            Photos = new Photos();
        }

        [Key]
        [Required]
        [HiddenInput]
        public uint Id { get; set; }

        public IEnumerable<string> PhotoUrls { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("My", "Cars"), ("Edit", "Cars") };

            return new Breadcrumb(paths);
        }
    }

    public class CarSpecifications
    {
        public uint Id { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Vin")]
        public string Vin { get; set; }

        [DisplayName("Vehicle Type")]
        public string VehicleType { get; set; }

        [DisplayName("Body Type")]
        public string BodyType { get; set; }

        [DisplayName("Make")]
        public string Make { get; set; }

        [DisplayName("Model")]
        public string Model { get; set; }

        [DisplayName("Variant")]
        public string Variant { get; set; }

        [DisplayName("Condition")]
        public string Condition { get; set; }

        [DisplayName("Production Date")]
        public string ProductionDate { get; set; }

        [DisplayName("Color")]
        public string Color { get; set; }

        [DisplayName("Fuel")]
        public string FuelType { get; set; }

        [DisplayName("Gearbox")]
        public string GearboxType { get; set; }

        [DisplayName("Horse Power")]
        public string HorsePower { get; set; }

        [DisplayName("CC")]
        public string Cc { get; set; }

        [DisplayName("Euro Standart")]
        public string EuroStandart { get; set; }

        [DisplayName("Price")]
        public string Price { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Specific Location")]
        public string SpecificLocation { get; set; }

        [DisplayName("Mileage")]
        public string Mileage { get; set; }

        [DisplayName("Door Number")]
        public string DoorNumber { get; set; }

        [DisplayName("Owner")]
        public string OwnerName { get; set; }

        [DisplayName("Mobile")]
        public string Mobile { get; set; }

        public IEnumerable<string> PhotoUrls { get; set; }

        [DisplayName("Air Conditioning")]
        public bool HasAirConditioning { get; set; }

        [DisplayName("Climatronic")]
        public bool HasClimatronic { get; set; }

        [DisplayName("Lether Interior")]
        public bool HasLetherInterior { get; set; }

        [DisplayName("Electric Windows")]
        public bool HasElectricWindows { get; set; }

        [DisplayName("Electric Mirrors")]
        public bool HasElectricMirrors { get; set; }

        [DisplayName("Electric Seats")]
        public bool HasElectricSeats { get; set; }

        [DisplayName("Seat Heating")]
        public bool HasSeatHeating { get; set; }

        [DisplayName("Sunroof")]
        public bool HasSunroof { get; set; }

        [DisplayName("Stereo")]
        public bool HasStereo { get; set; }

        [DisplayName("Alloy Wheels")]
        public bool HasAlloyWheels { get; set; }

        [DisplayName("Dvd/Tv")]
        public bool HasDvdTv { get; set; }

        [DisplayName("Multi Steering Wheel")]
        public bool HasMultiSteeringWheel { get; set; }

        [DisplayName("4WD")]
        public bool HasAllWheelDrive { get; set; }

        [DisplayName("ABS")]
        public bool HasAbs { get; set; }

        [DisplayName("ESP")]
        public bool HasEsp { get; set; }

        [DisplayName("AirBag")]
        public bool HasAirBag { get; set; }

        [DisplayName("Xenon Lights")]
        public bool HasXenonLights { get; set; }

        [DisplayName("Halogen Lights")]
        public bool HasHalogenHeadlights { get; set; }

        [DisplayName("Traction Control")]
        public bool HasTractionControl { get; set; }

        [DisplayName("Parktronic")]
        public bool HasParktronic { get; set; }

        [DisplayName("Alarm")]
        public bool HasAlarm { get; set; }

        [DisplayName("Imobilizer")]
        public bool HasImmobilizer { get; set; }

        [DisplayName("Central Lock")]
        public bool HasCentralLock { get; set; }

        [DisplayName("Insurance")]
        public bool HasInsurance { get; set; }

        [DisplayName("Armored")]
        public bool IsArmored { get; set; }

        [DisplayName("Keyless")]
        public bool IsKeyless { get; set; }

        [DisplayName("Tiptronic/Multitronic")]
        public bool IsTiptronicMultitronic { get; set; }

        [DisplayName("Autopilot")]
        public bool HasAutopilot { get; set; }

        [DisplayName("Power Steering")]
        public bool HasPowerSteering { get; set; }

        [DisplayName("Onboard Computer")]
        public bool HasOnboardComputer { get; set; }

        [DisplayName("Service Book")]
        public bool HasServiceBook { get; set; }

        [DisplayName("Warranty")]
        public bool HasWarranty { get; set; }

        [DisplayName("Navigation System")]
        public bool HasNavigationSystem { get; set; }

        [DisplayName("Right Hand Drive")]
        public bool IsRightHandDrive { get; set; }

        [DisplayName("Tuning")]
        public bool HasTuning { get; set; }

        [DisplayName("Panoramic Roof")]
        public bool HasPanoramicRoof { get; set; }

        [DisplayName("Taxi")]
        public bool IsTaxi { get; set; }

        [DisplayName("Retro")]
        public bool IsRetro { get; set; }

        [DisplayName("Tow")]
        public bool HasTow { get; set; }

        [DisplayName("More Seats")]
        public bool HasMoreSeats { get; set; }

        [DisplayName("Refrigerator")]
        public bool HasRefrigerator { get; set; }
    }

    public class CarDetailsModel : CarSpecifications 
    {
        [DisplayName("Views")]
        public int ViewsCount { get; set; }

        public bool IsFavourite { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("Search", "Cars"), ("Details", "Cars") };

            return new Breadcrumb(paths);
        }
    }

    public class CarCompareModel
    {
        public List<CarSpecifications> Cars { get; set; }
    }

    public class SimpleCarModel
    {
        public uint Id { get; set; }

        public string MakeModel { get; set; }

        public string PhotoUrl { get; set; }
    }

    public class Photos
    {
        [DisplayName("Images")]
        public IFormFile Photo1 { get; set; }
        public string Photo1Url { get; set; }

        public IFormFile? Photo2 { get; set; }
        public string Photo2Url { get; set; }

        public IFormFile? Photo3 { get; set; }
        public string Photo3Url { get; set; }

        public IFormFile? Photo4 { get; set; }
        public string Photo4Url { get; set; }

        public IFormFile? Photo5 { get; set; }
        public string Photo5Url { get; set; }

        public IFormFile? Photo6 { get; set; }
        public string Photo6Url { get; set; }

        public IFormFile? Photo7 { get; set; }
        public string Photo7Url { get; set; }

        public IFormFile? Photo8 { get; set; }
        public string Photo8Url { get; set; }

        public IFormFile? Photo9 { get; set; }
        public string Photo9Url { get; set; }

        public IFormFile? Photo10 { get; set; }
        public string Photo10Url { get; set; }

        public IFormFile? Photo11 { get; set; }
        public string Photo11Url { get; set; }

        public IFormFile? Photo12 { get; set; }
        public string Photo12Url { get; set; }

        public void LoadPhotoUrls(uint carId)
        {
            var carPhotos = Db.CarPhotos.GetByCarId(carId);
            var props = this.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)).ToList();

            for (int i = 0; i < carPhotos.Count(); i++)
            {
                props[i].SetValue(this, carPhotos[i].Url);
            }
        }
    }
}
