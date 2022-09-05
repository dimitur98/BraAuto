using BraAutoDb.Models.Search;

namespace BraAutoDb.Models.CarsSearch
{
    public class Request : BaseRequest
    {
        public uint? VehicleTypeId { get; set; }

        public IEnumerable<uint> ConditionIds { get; set; }

        public uint? MakeId { get; set; }

        public IEnumerable<uint> ModelIds { get; set; }
        
        public IEnumerable<uint> BodyTypeIds { get; set; }
        
        public IEnumerable<uint> ColorIds { get; set; }

        public IEnumerable<uint> FuelTypeIds { get; set; }
        
        public DateTime? ProductionDateFrom { get; set; }

        public DateTime? ProductionDateTo { get; set; }

        public uint? HorsePowerFrom { get; set; }
        
        public IEnumerable<uint> EuroStandartIds { get; set; }

        public IEnumerable<uint> GearboxTypeIds { get; set; }
        
        public uint? PriceFrom { get; set; }

        public uint? PriceTo { get; set; }

        public IEnumerable<uint> LocationIds { get; set; }

        public IEnumerable<uint> DoorNumberIds { get; set; }
        
        public bool? HasAirConditioning { get; set; }
        
        public bool? HasClimatronic { get; set; }
        
        public bool? HasLetherInterior { get; set; }
        
        public bool? HasElectricWindows { get; set; }
        
        public bool? HasElectricMirrors { get; set; }
        
        public bool? HasElectricSeats { get; set; }
        
        public bool? HasSeatHeating { get; set; }
        
        public bool? HasSunroof { get; set; }
        
        public bool? HasStereo { get; set; }

        public bool? HasAlloyWheels { get; set; }
        
        public bool? HasDvdTv { get; set; }
        
        public bool? HasMultiStearingWheel { get; set; }
        
        public bool? HasAllWheelDrive { get; set; }
        
        public bool? HasAbs { get; set; }
        
        public bool? HasEsp { get; set; }
        
        public bool? HasAirBag { get; set; }
        
        public bool? HasXenonLights { get; set; }
        
        public bool? HasHalogenHeadlights { get; set; }
        
        public bool? HasTractionControl { get; set; }
        
        public bool? HasParktronic { get; set; }
        
        public bool? HasAlarm { get; set; }
        
        public bool? HasImobilizer { get; set; }
        
        public bool? HasCentralLock { get; set; }
        
        public bool? HasInsurance { get; set; }
        
        public bool? IsArmored { get; set; }
        
        public bool? IsKeyless { get; set; }
        
        public bool? IsTiptronicMultitronic { get; set; }
        
        public bool? HasAutopilot { get; set; }
        
        public bool? HasPowerSteering { get; set; }
        
        public bool? HasOnboardComputer { get; set; }
        
        public bool? HasServiceBook { get; set; }
        
        public bool? HasWarranty { get; set; }
        
        public bool? HasNavigationSystem { get; set; }
        
        public bool? IsRightHandDrive { get; set; }
        
        public bool? HasTuning { get; set; }
        
        public bool? HasPanoramicRoof { get; set; }
        
        public bool? IsTaxi { get; set; }
        
        public bool? IsRetro { get; set; }
        
        public bool? HasTow { get; set; }
        
        public bool? HasMoreSeats { get; set; }
        
        public bool? HasRefrigerator { get; set; }
        
        public bool? IsApproved { get; set; }
        
        public bool? IsAdvert { get; set; }
    }
}
