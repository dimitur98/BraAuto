using BraAutoDb.Dal;
using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "car")]
    public class Car : AuditInfo<uint>
    {
        [Column(Name = "description")]
        public string Description { get; set; }

        [Column(Name = "vin")]
        public string Vin { get; set; }

        [Column(Name = "vehicle_type_id")]
        public uint VehicleTypeId { get; set; }

        [Column(Name = "condition_id")]
        public uint ConditionId { get; set; }

        [Column(Name = "model_id")]
        public uint ModelId { get; set; }
        public Model Model { get; set; }

        [Column(Name = "variant")]
        public string Variant { get; set; }

        [Column(Name = "body_type_id")]
        public uint BodyTypeId { get; set; }

        [Column(Name = "color_id")]
        public uint ColorId { get; set; }

        [Column(Name = "fuel_type_id")]
        public uint FuelTypeId { get; set; }

        [Column(Name = "production_date")]
        public DateTime ProductionDate { get; set; }

        [Column(Name = "horse_power")]
        public uint? HorsePower { get; set; }

        [Column(Name = "cc")]
        public uint? CC { get; set; }

        [Column(Name = "euro_standart_id")]
        public uint EuroStandartId { get; set; }

        [Column(Name = "gearbox_type_id")]
        public uint GearboxTypeId { get; set; }
        public GearboxType GearboxType { get; set; }

        [Column(Name = "price")]
        public uint Price { get; set; }

        [Column(Name = "location_id")]
        public uint LocationId { get; set; }

        [Column(Name = "specific_location")]
        public string SpecificLocation { get; set; }

        [Column(Name = "mileage")]
        public uint Mileage { get; set; }

        [Column(Name = "door_number_id")]
        public uint DoorNumberId { get; set; }

        public IEnumerable<string> ImgUrls { get; set; }

        [Column(Name = "has_air_conditioning")]
        public bool HasAirConditioning { get; set; }

        [Column(Name = "has_climatronic")]
        public bool HasClimatronic { get; set; }

        [Column(Name = "has_lether_interior")]
        public bool HasLetherInterior { get; set; }

        [Column(Name = "has_electric_windows")]
        public bool HasElectricWindows { get; set; }

        [Column(Name = "has_electric_mirrors")]
        public bool HasElectricMirrors { get; set; }

        [Column(Name = "has_electric_seats")]
        public bool HasElectricSeats { get; set; }

        [Column(Name = "has_seat_heating")]
        public bool HasSeatHeating { get; set; }

        [Column(Name = "has_sunroof")]
        public bool HasSunroof { get; set; }

        [Column(Name = "has_stereo")]
        public bool HasStereo { get; set; }

        [Column(Name = "has_alloy_wheels")]
        public bool HasAlloyWheels { get; set; }

        [Column(Name = "has_dvd_tv")]
        public bool HasDvdTv { get; set; }

        [Column(Name = "has_multi_steering_wheel")]
        public bool HasMultiSteeringWheel { get; set; }

        [Column(Name = "has_all_wheel_drive")]
        public bool HasAllWheelDrive { get; set; }

        [Column(Name = "has_abs")]
        public bool HasAbs { get; set; }

        [Column(Name = "has_esp")]
        public bool HasEsp { get; set; }

        [Column(Name = "has_airbag")]
        public bool HasAirBag { get; set; }

        [Column(Name = "has_xenon_lights")]
        public bool HasXenonLights { get; set; }

        [Column(Name = "has_halogen_headlights")]
        public bool HasHalogenHeadlights { get; set; }

        [Column(Name = "has_traction_control")]
        public bool HasTractionControl { get; set; }

        [Column(Name = "has_parktronic")]
        public bool HasParktronic { get; set; }

        [Column(Name = "has_alarm")]
        public bool HasAlarm { get; set; }

        [Column(Name = "has_immobilizer")]
        public bool HasImmobilizer { get; set; }

        [Column(Name = "has_central_lock")]
        public bool HasCentralLock { get; set; }

        [Column(Name = "has_insurance")]
        public bool HasInsurance { get; set; }

        [Column(Name = "is_armored")]
        public bool IsArmored { get; set; }

        [Column(Name = "is_keyless")]
        public bool IsKeyless { get; set; }

        [Column(Name = "is_tiptronic_multitronic")]
        public bool IsTiptronicMultitronic { get; set; }

        [Column(Name = "has_autopilot")]
        public bool HasAutopilot { get; set; }

        [Column(Name = "has_power_steering")]
        public bool HasPowerSteering { get; set; }

        [Column(Name = "has_onboard_computer")]
        public bool HasOnboardComputer { get; set; }

        [Column(Name = "has_service_book")]
        public bool HasServiceBook { get; set; }

        [Column(Name = "has_warranty")]
        public bool HasWarranty { get; set; }

        [Column(Name = "has_navigation_system")]
        public bool HasNavigationSystem { get; set; }

        [Column(Name = "is_right_hand_drive")]
        public bool IsRightHandDrive { get; set; }

        [Column(Name = "has_tuning")]
        public bool HasTuning { get; set; }

        [Column(Name = "has_panoramic_roof")]
        public bool HasPanoramicRoof { get; set; }

        [Column(Name = "is_taxi")]
        public bool IsTaxi { get; set; }

        [Column(Name = "is_retro")]
        public bool IsRetro { get; set; }

        [Column(Name = "has_tow")]
        public bool HasTow { get; set; }

        [Column(Name = "has_more_seats")]
        public bool HasMoreSeats { get; set; }

        [Column(Name = "has_refrigerator")]
        public bool HasRefrigerator { get; set; }

        [Column(Name = "is_approved")]
        public bool IsApproved { get; set; }

        [Column(Name = "is_advert")]
        public bool IsAdvert { get; set; }

        public void LoadModel()
        {
            this.Model = Db.Models.GetById(this.ModelId);
        }

        public void LoadGearboxType()
        {
            this.GearboxType = Db.GearboxTypes.GetById(this.GearboxTypeId);
        }
    }
}
