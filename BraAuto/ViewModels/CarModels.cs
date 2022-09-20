using BraAutoDb.Models;
using BraAutoDb.Models.CarsSearch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BraAuto.ViewModels
{
    public abstract class CarSearchBaseModel : BaseSearchModel<Response, Car>
    {
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

        public Request ToSearchRequest()
        {
            var request = new Request
            {
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
        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("Search", "Cars") };

            return new Breadcrumb(paths);
        }
    }
    public class HomeCarSearchModel : CarSearchBaseModel { }

    public class MyCarModel : CarSearchBaseModel
    {
        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("My", "Cars") };

            return new Breadcrumb(paths);
        }
    }

    public class CarAdminModel : CarSearchBaseModel
    {
        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("Admin", "Cars") };

            return new Breadcrumb(paths);
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

        public Imgs Imgs { get; set; }

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
        [Key]
        [Required]
        [HiddenInput]
        public uint Id { get; set; }

        public IEnumerable<string> ImgUrls { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("My", "Cars"), ("Edit", "Cars") };

            return new Breadcrumb(paths);
        }
    }

    public class DetailsCarModel
    {
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

    public class Imgs
    {
        [Required]
        [DisplayName("Images")]
        public IFormFile ImgMain { get; set; }
        public string ImgMainUrl { get; set; }

        public IFormFile? Img2 { get; set; }
        public string Img2Url { get; set; }

        public IFormFile? Img3 { get; set; }
        public string Img3Url { get; set; }

        public IFormFile? Img4 { get; set; }
        public string Img4Url { get; set; }

        public IFormFile? Img5 { get; set; }
        public string Img5Url { get; set; }

        public IFormFile? Img6 { get; set; }
        public string Img6Url { get; set; }

        public IFormFile? Img7 { get; set; }
        public string Img7Url { get; set; }

        public IFormFile? Img8 { get; set; }
        public string Img8Url { get; set; }

        public IFormFile? Img9 { get; set; }
        public string Img9Url { get; set; }

        public IFormFile? Img10 { get; set; }
        public string Img10Url { get; set; }

        public IFormFile? Img11 { get; set; }
        public string Img11Url { get; set; }

        public IFormFile? Img12 { get; set; }
        public string Img12Url { get; set; }
    }
}
