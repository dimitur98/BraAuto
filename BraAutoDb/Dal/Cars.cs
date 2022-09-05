using BraAuto.Helpers.Extensions;
using BraAutoDb.Models;
using BraAutoDb.Models.CarsSearch;
using SqlQueryBuilder.MySql;
using System.Data;


namespace BraAutoDb.Dal
{
    public static class Cars
    {
        public static Response Search(Request request)
        {
            var query = new Query
            {
                Select = new List<string>() { "c.*" },
                From = "car AS c",
                Where = new List<string>() { "1 = 1" },
                Joins = new List<string>()
            };

            //set default sorting
            if (string.IsNullOrEmpty(request.SortColumn))
            {
                request.SortColumn = "c.created_at";
                request.SortDesc = false;
            }

            query.OrderBys = new List<OrderBy>() { new OrderBy(request.SortColumn, request.SortDesc) };

            if (request.VehicleTypeId != null) { query.Where.Add(" AND c.vehicle_type_id = @vehicleTypeId"); }
            if (!request.ConditionIds.IsNullOrEmpty()) { query.Where.Add(" AND c.condition_id IN @conditionIds"); }
            if (request.MakeId != null) { query.Where.Add(" AND EXISTS (SELECT * FROM `model` m WHERE m.id = c.model_id AND m.make_id = @makeId)"); }
            if (!request.ModelIds.IsNullOrEmpty()) { query.Where.Add(" AND c.model_id IN @modelIds"); }
            if (!request.BodyTypeIds.IsNullOrEmpty()) { query.Where.Add(" AND c.body_type_id IN @bodyTypeIds"); }
            if (!request.ColorIds.IsNullOrEmpty()) { query.Where.Add(" AND c.color_id IN @colorIds"); }
            if (!request.FuelTypeIds.IsNullOrEmpty()) { query.Where.Add(" AND c.fuel_type_id IN @fuelTypeIds"); }
            if (request.ProductionDateFrom.HasValue) { query.Where.Add(" AND c.production_date >= DATE(@productionDateFrom)"); }
            if (request.ProductionDateTo.HasValue) { query.Where.Add(" AND c.production_date <= DATE(@productionDateTo)"); }
            if (request.HorsePowerFrom != null) { query.Where.Add(" AND c.horse_power >= @horsePowerFrom"); }
            if (!request.EuroStandartIds.IsNullOrEmpty()) { query.Where.Add(" AND c.euro_standart_id IN @euroStandartIds"); }
            if (!request.GearboxTypeIds.IsNullOrEmpty()) { query.Where.Add(" AND c.gearbox_type_id IN @gearboxTypeIds"); }
            if (request.PriceFrom != null) { query.Where.Add(" AND c.price >= @priceFrom"); }
            if (request.PriceTo != null) { query.Where.Add(" AND c.price <= @priceTo"); }
            if (!request.LocationIds.IsNullOrEmpty()) { query.Where.Add(" AND c.location_id IN @locationIds"); }
            if (!request.DoorNumberIds.IsNullOrEmpty()) { query.Where.Add(" AND c.door_number_id IN @doorNumberIds"); }
            if (request.HasAirConditioning.HasValue) { query.Where.Add("AND c.has_air_conditioning = @hasAirConditioning"); }
            if (request.HasClimatronic.HasValue) { query.Where.Add(" AND c.has_climatronic = @hasClimatronic"); }
            if (request.HasLetherInterior.HasValue) { query.Where.Add(" AND c.has_lether_interior = @hasLetherInterior"); }
            if (request.HasElectricWindows.HasValue) { query.Where.Add(" AND c.has_electric_windows = @hasElectricWindow"); }
            if (request.HasElectricMirrors.HasValue) { query.Where.Add(" AND c.has_electric_mirrors = @hasElectricMirors"); }
            if (request.HasElectricSeats.HasValue) { query.Where.Add(" AND c.has_electric_seats = @hasElectricSeats"); }
            if (request.HasSeatHeating.HasValue) { query.Where.Add(" AND c.has_seat_heating = @hasSeatHeating"); }
            if (request.HasSunroof.HasValue) { query.Where.Add(" AND c.has_sunroof = @hasSunroof"); }
            if (request.HasStereo.HasValue) { query.Where.Add(" AND c.has_stereo = @hasStereo"); }
            if (request.HasAlloyWheels.HasValue) { query.Where.Add(" AND c.has_alloy_wheels = @hasAlloyWheels"); }
            if (request.HasDvdTv.HasValue) { query.Where.Add(" AND c.has_dvd_tv = @hasDvdTv"); }
            if (request.HasMultiStearingWheel.HasValue) { query.Where.Add(" AND c.has_multi_steering_wheel = @hasMultiSteeringWheel"); }
            if (request.HasAllWheelDrive.HasValue) { query.Where.Add(" AND c.has_all_wheel_drive = @hasAllWheelDrive"); }
            if (request.HasAbs.HasValue) { query.Where.Add(" AND c.has_abs = @hasAbs"); }
            if (request.HasEsp.HasValue) { query.Where.Add(" AND c.has_esp = @hasEsp"); }
            if (request.HasAirBag.HasValue) { query.Where.Add(" AND c.has_airbag = @hasAirbag"); }
            if (request.HasXenonLights.HasValue) { query.Where.Add(" AND c.has_xenon_lights = @hasXenonLights"); }
            if (request.HasHalogenHeadlights.HasValue) { query.Where.Add(" AND c.has_halogen_headlights = @hasHalogenHeadlights"); }
            if (request.HasTractionControl.HasValue) { query.Where.Add(" AND c.has_traction_control = @hasTractionControl"); }
            if (request.HasParktronic.HasValue) { query.Where.Add(" AND c.has_parktronic = @hasParktronic"); }
            if (request.HasAlarm.HasValue) { query.Where.Add(" AND c.has_alarm = @hasAlarm"); }
            if (request.HasImobilizer.HasValue) { query.Where.Add(" AND c.has_immobilizer = @hasImmobilizer"); }
            if (request.HasCentralLock.HasValue) { query.Where.Add(" AND c.has_central_lock = @hasCentralLock"); }
            if (request.HasInsurance.HasValue) { query.Where.Add(" AND c.has_insurance = @hasInsurance"); }
            if (request.IsArmored.HasValue) { query.Where.Add(" AND c.is_armored = @isArmored"); }
            if (request.IsKeyless.HasValue) { query.Where.Add(" AND c.is_keyless = @isKeyless"); }
            if (request.IsTiptronicMultitronic.HasValue) { query.Where.Add(" AND c.is_tiptronic_multitronic = @isTiptronicMultitronic"); }
            if (request.HasAutopilot.HasValue) { query.Where.Add(" AND c.has_autopilot = @hasAutopilot"); }
            if (request.HasPowerSteering.HasValue) { query.Where.Add(" AND c.has_power_steering = @hasPowerSteering"); }
            if (request.HasOnboardComputer.HasValue) { query.Where.Add(" AND c.has_onboard_computer = @hasOnboardComputer"); }
            if (request.HasServiceBook.HasValue) { query.Where.Add(" AND c.has_service_book = @hasServiceBook"); }
            if (request.HasWarranty.HasValue) { query.Where.Add(" AND c.has_warranty = @hasWarranty"); }
            if (request.HasNavigationSystem.HasValue) { query.Where.Add(" AND c.has_navigation_system = @hasNavigationSystem"); }
            if (request.IsRightHandDrive.HasValue) { query.Where.Add(" AND c.is_right_hand_drive = @isRightHandDrive"); }
            if (request.HasTuning.HasValue) { query.Where.Add(" AND c.has_tuning = @hasTuning"); }
            if (request.HasPanoramicRoof.HasValue) { query.Where.Add(" AND c.has_panoramic_roof = @hasPanoramicRoof"); }
            if (request.IsTaxi.HasValue) { query.Where.Add(" AND c.is_taxi = @isTaxi"); }
            if (request.IsRetro.HasValue) { query.Where.Add(" AND c.is_retro = @isRetro"); }
            if (request.HasTow.HasValue) { query.Where.Add(" AND c.has_tow = @hasTow"); }
            if (request.HasMoreSeats.HasValue) { query.Where.Add(" AND c.has_more_seats = @hasMoreSeats"); }
            if (request.HasRefrigerator.HasValue) { query.Where.Add(" AND c.has_refrigerated = @hasRefrigerated"); }
            if (request.IsApproved.HasValue) { query.Where.Add(" AND c.is_approved = @isApproved"); }
            if (request.IsAdvert.HasValue) { query.Where.Add(" AND c.is_advert = @isAdvert"); }

            if (request.IsActive != null) { query.Where.Add(" AND c.is_active = @isActive"); }

            query.Limit = new Limit(request.Offset, request.RowCount);

            var response = new Response();

            using (var connection = Db.Mapper.GetConnection())
            {
                var queryParams = new
                {
                    vehicleTypeId = request.VehicleTypeId,
                    conditionIds = request.ConditionIds,
                    makeId = request.MakeId,
                    modelIds = request.ModelIds,
                    bodyTypeIds = request.BodyTypeIds,
                    colorIds = request.ColorIds,
                    fuelTypeIds = request.FuelTypeIds,
                    productionDateFrom = request.ProductionDateFrom,
                    productionDateTo = request.ProductionDateTo,
                    horsePowerFrom = request.HorsePowerFrom,
                    euroStandartIds = request.EuroStandartIds,
                    gearboxTypeIds = request.GearboxTypeIds,
                    priceFrom = request.PriceFrom,
                    priceTo = request.PriceTo,
                    locationIds = request.LocationIds,
                    doorNumberIds = request.DoorNumberIds,
                    hasAirConditioning = request.HasAirConditioning,
                    hasClimatronic = request.HasClimatronic,
                    hasLetherInterior = request.HasLetherInterior,
                    hasElectricWindows = request.HasElectricWindows,
                    hasElectricMirrors = request.HasElectricMirrors,
                    hasElectricSeats = request.HasElectricSeats,
                    hasSeatHeating = request.HasSeatHeating,
                    hasSunroof = request.HasSunroof,
                    hasStereo = request.HasStereo,
                    hasAlloyWheels = request.HasAlloyWheels,
                    hasDvdTv = request.HasDvdTv,
                    hasMultiStearingWheel = request.HasMultiStearingWheel,
                    hasAllWheelDrive = request.HasAllWheelDrive,
                    hasAbs = request.HasAbs,
                    hasEsp = request.HasEsp,
                    hasAirBag = request.HasAirBag,
                    hasXenonLights = request.HasXenonLights,
                    hasHalogenHeadlights = request.HasHalogenHeadlights,
                    hasTractionControl = request.HasTractionControl,
                    hasParktronic = request.HasParktronic,
                    hasAlarm = request.HasAlarm,
                    hasImobilizer = request.HasImobilizer,
                    hasCentralLock = request.HasCentralLock,
                    hasInsurance = request.HasInsurance,
                    isArmored = request.IsArmored,
                    isKeyless = request.IsKeyless,
                    isTiptronicMultitronic = request.IsTiptronicMultitronic,
                    hasAutopilot = request.HasAutopilot,
                    hasPowerSteering = request.HasPowerSteering,
                    hasOnboardComputer = request.HasOnboardComputer,
                    hasServiceBook = request.HasServiceBook,
                    hasWarranty = request.HasWarranty,
                    hasNavigationSystem = request.HasNavigationSystem,
                    isRightHandDrive = request.IsRightHandDrive,
                    hasTuning = request.HasTuning,
                    hasPanoramicRoof = request.HasPanoramicRoof,
                    isTaxi = request.IsTaxi,
                    isRetro = request.IsRetro,
                    hasTow = request.HasTow,
                    hasMoreSeats = request.HasMoreSeats,
                    hasRefrigerator = request.HasRefrigerator,
                    isApproved = request.IsApproved,
                    isAdvert = request.IsAdvert,
                };

                //get TotalRecordsCount
                if (request.ReturnTotalRecords)
                {
                    response.TotalRecords = Db.QueryCount(connection, query, param: queryParams);

                    if (response.TotalRecords <= 0) { return response; }
                }

                response.Records = Db.Mapper.Query<Car>(connection, query.Build(), queryParams, commandType: CommandType.Text);
            }

            return response;
        }

        public static Car GetById(uint id)
        {
            var sql = "SELECT * FROM car WHERE id = @id";

            return Db.Mapper.Query<Car>(sql, new { id }).FirstOrDefault();
        }

        public static void Insert(Car car)
        {
            var sql = @"INSERT INTO `car`
                        (
                            `vehicle_type_id`,
                            `condition_id`,
                            `model_id`,
                            `variant`,
                            `body_type_id`,
                            `color_id`,
                            `fuel_type_id`,
                            `production_date`,
                            `horse_power`,
                            `cc`,
                            `euro_standart_id`,
                            `gearbox_type_id`,
                            `price`,
                            `location_id`,
                            `specific_location`,
                            `mileage`,
                            `door_number_id`,
                            `has_air_conditioning`,
                            `has_climatronic`,
                            `has_lether_interior`,
                            `has_electric_windows`,
                            `has_electric_mirrors`,
                            `has_electric_seats`,
                            `has_seat_heating`,
                            `has_sunroof`,
                            `has_stereo`,
                            `has_alloy_wheels`,
                            `has_dvd_tv`,
                            `has_multi_steering_wheel`,
                            `has_all_wheel_drive`,
                            `has_abs`,
                            `has_esp`,
                            `has_airbag`,
                            `has_xenon_lights`,
                            `has_halogen_headlights`,
                            `has_traction_control`,
                            `has_parktronic`,
                            `has_alarm`,
                            `has_immobilizer`,
                            `has_central_lock`,
                            `has_insurance`,
                            `is_armored`,
                            `is_keyless`,
                            `is_tiptronic_multitronic`,
                            `has_autopilot`,
                            `has_power_steering`,
                            `has_onboard_computer`,
                            `has_service_book`,
                            `has_warranty`,
                            `has_navigation_system`,
                            `is_right_hand_drive`,
                            `has_tuning`,
                            `has_panoramic_roof`,
                            `is_taxi`,
                            `is_retro`,
                            `has_tow`,
                            `has_more_seats`,
                            `has_refrigerated`,
                            `is_approved`,
                            `is_advert`,
                            `creator_id`,
                            `created_at`,
                            `editor_id`,
                            `edited_at`
                        )VALUES(
                            @vehicleTypeId,
                            @conditionId,
                            @modelId,
                            @variant,
                            @bodyTypeId,
                            @colorId,
                            @fuelTypeId,
                            @productionDate,
                            @horsePower,
                            @cc,
                            @euroStandartId,
                            @gearboxTypeId,
                            @price,
                            @locationId,
                            @specificLocation,
                            @mileage,
                            @doorNumberId,
                            @hasAirConditioning, 
                            @hasClimatronic, 
                            @hasLetherInterior, 
                            @hasElectricWindows, 
                            @hasElectricMirrors,
                            @hasElectricSeats, 
                            @hasSeatHeating, 
                            @hasSunroof, 
                            @hasStereo, 
                            @hasAlloyWheels, 
                            @hasDvdTv, 
                            @hasMultiStearingWheel, 
                            @hasAllWheelDrive, 
                            @hasAbs, 
                            @hasEsp, 
                            @hasAirBag, 
                            @hasXenonLights, 
                            @hasHalogenHeadlights, 
                            @hasTractionControl, 
                            @hasParktronic,
                            @hasAlarm, 
                            @hasImobilizer, 
                            @hasCentralLock, 
                            @hasInsurance, 
                            @isArmored, 
                            @isKeyless, 
                            @isTiptronicMultitronic, 
                            @hasAutopilot, 
                            @hasPowerSteering, 
                            @hasOnboardComputer, 
                            @hasServiceBook, 
                            @hasWarranty, 
                            @hasNavigationSystem, 
                            @isRightHandDrive, 
                            @hasTuning, 
                            @hasPanoramicRoof, 
                            @isTaxi, 
                            @isRetro, 
                            @hasTow, 
                            @hasMoreSeats, 
                            @hasRefrigerator, 
                            @isApproved,
                            @isAdvert,
                            @creatorId,
                            NOW(),
                            @editorId,
                            NOW()
                        )

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                vehicleTypeId = car.VehicleTypeId,
                conditionId = car.ConditionId,
                modelId = car.ModelId,
                variant = car.Variant,
                bodyTypeId = car.BodyTypeId,
                colorId = car.ColorId,
                fuelTypeId = car.FuelTypeId,
                productionDate = car.ProductionDate,
                horsePower = car.HorsePower,
                cc = car.CC,
                euroStandartId = car.EuroStandartId,
                gearboxTypeId = car.GearboxTypeId,
                price = car.Price,
                locationId = car.LocationId,
                specificLocation = car.SpecificLocation,
                mileage = car.Mileage,
                doorNumberId = car.DoorNumberId,
                hasAirConditioning = car.HasAirConditioning,
                hasClimatronic = car.HasClimatronic,
                hasLetherInterior = car.HasLetherInterior,
                hasElectricWindows = car.HasElectricWindows,
                hasElectricMirrors = car.HasElectricMirrors,
                hasElectricSeats = car.HasElectricSeats,
                hasSeatHeating = car.HasSeatHeating,
                hasSunroof = car.HasSunroof,
                hasStereo = car.HasStereo,
                hasAlloyWheels = car.HasAlloyWheels,
                hasDvdTv = car.HasDvdTv,
                hasMultiStearingWheel = car.HasMultiStearingWheel,
                hasAllWheelDrive = car.HasAllWheelDrive,
                hasAbs = car.HasAbs,
                hasEsp = car.HasEsp,
                hasAirBag = car.HasAirBag,
                hasXenonLights = car.HasXenonLights,
                hasHalogenHeadlights = car.HasHalogenHeadlights,
                hasTractionControl = car.HasTractionControl,
                hasParktronic = car.HasParktronic,
                hasAlarm = car.HasAlarm,
                hasImobilizer = car.HasImobilizer,
                hasCentralLock = car.HasCentralLock,
                hasInsurance = car.HasInsurance,
                isArmored = car.IsArmored,
                isKeyless = car.IsKeyless,
                isTiptronicMultitronic = car.IsTiptronicMultitronic,
                hasAutopilot = car.HasAutopilot,
                hasPowerSteering = car.HasPowerSteering,
                hasOnboardComputer = car.HasOnboardComputer,
                hasServiceBook = car.HasServiceBook,
                hasWarranty = car.HasWarranty,
                hasNavigationSystem = car.HasNavigationSystem,
                isRightHandDrive = car.IsRightHandDrive,
                hasTuning = car.HasTuning,
                hasPanoramicRoof = car.HasPanoramicRoof,
                isTaxi = car.IsTaxi,
                isRetro = car.IsRetro,
                hasTow = car.HasTow,
                hasMoreSeats = car.HasMoreSeats,
                hasRefrigerator = car.HasRefrigerator,
                isApproved = car.IsApproved,
                isAdvert = car.IsAdvert,
                creatorId = 0,
                editorId = 0
            };

            car.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public static void Update(Car car)
        {
            var sql = @"UPDATE `car`
                            SET vehicle_type_id = @vehicleTypeId,
                                condition_id = @conditionId,
                                model_id = @modelId,
                                variant = @variant,
                                body_type_id = @bodyTypeId,
                                color_id = @colorId,
                                fuel_type_id = @fuelTypeId,
                                production_date = @productionDate,
                                horse_power @horsePower,
                                cc = @cc,
                                euro_standart_id = @euroStandartId,
                                gearbox_type_id = @gearboxTypeId,
                                price = @price,
                                location_id = @locationId,
                                specific_location = @specificLocation,
                                mileage = @mileage,
                                door_number_id = @doorNumber,
                                has_air_conditioning = @hasAirConditioning,
                                has_climatronic = @hasClimatronic,
                                has_lether_interior = @hasLetherInterior`,
                                has_electric_windows = @hasElectricWindow,
                                has_electric_mirrors = @hasElectricMirors,
                                has_electric_seats = @hasElectricSeats,
                                has_seat_heating = @hasSeatHeating,
                                has_sunroof = @hasSunroof,
                                has_stereo = @hasStereo,
                                has_alloy_wheels = @hasAlloyWheels,
                                has_dvd_tv = @hasDvdTv,
                                has_multi_steering_wheel = @hasMultiSteeringWheel,
                                has_all_wheel_drive = @hasAllWheelDrive,
                                has_abs = @hasAbs,
                                has_esp = @hasEsp,
                                has_airbag = @hasAirbag,
                                has_xenon_lights = @hasXenonLights,
                                has_halogen_headlights = @hasHalogenHeadlights,
                                has_traction_control = @hasTractionControl,
                                has_parktronic = @hasParktronic,
                                has_alarm = @hasAlarm,
                                has_immobilizer = @hasImmobilizer,
                                has_central_lock = @hasCentralLock,
                                has_insurance = @hasInsurance,
                                is_armored = @isArmored,
                                is_keyless = @isKeyless,
                                is_tiptronic_multitronic = @isTiptronicMultitronic,
                                has_autopilot = @hasAutopilot,
                                has_power_steering = @hasPowerSteering,
                                has_onboard_computer = @hasOnboardComputer,
                                has_service_book = @hasServiceBook,
                                has_warranty = @hasWarranty,
                                has_navigation_system = @hasNavigationSystem,
                                is_right_hand_drive = @isRightHandDrive,
                                has_tuning = @hasTuning,
                                has_panoramic_roof = @hasPanoramicRoof,
                                is_taxi = @isTaxi,
                                is_retro = @isRetro,
                                has_tow = @hasTow,
                                has_more_seats = @hasMoreSeats,
                                has_refrigerated = @hasRefrigerated,
                                is_approved = @isApproved,
                                is_advert = @isAdvert,
                                editor_id = @editorId,
                                edited_at = NOW()
                        WHERE id = @id";

            var queryParams = new
            {
                id = car.Id,
                vehicleTypeId = car.VehicleTypeId,
                conditionId = car.ConditionId,
                modelId = car.ModelId,
                variant = car.Variant,
                bodyTypeId = car.BodyTypeId,
                colorId = car.ColorId,
                fuelTypeId = car.FuelTypeId,
                productionDate = car.ProductionDate,
                horsePower = car.HorsePower,
                cc = car.CC,
                euroStandartId = car.EuroStandartId,
                gearboxTypeId = car.GearboxTypeId,
                price = car.Price,
                locationId = car.LocationId,
                specificLocation = car.SpecificLocation,
                mileage = car.Mileage,
                doorNumberId = car.DoorNumberId,
                hasAirConditioning = car.HasAirConditioning,
                hasClimatronic = car.HasClimatronic,
                hasLetherInterior = car.HasLetherInterior,
                hasElectricWindows = car.HasElectricWindows,
                hasElectricMirrors = car.HasElectricMirrors,
                hasElectricSeats = car.HasElectricSeats,
                hasSeatHeating = car.HasSeatHeating,
                hasSunroof = car.HasSunroof,
                hasStereo = car.HasStereo,
                hasAlloyWheels = car.HasAlloyWheels,
                hasDvdTv = car.HasDvdTv,
                hasMultiStearingWheel = car.HasMultiStearingWheel,
                hasAllWheelDrive = car.HasAllWheelDrive,
                hasAbs = car.HasAbs,
                hasEsp = car.HasEsp,
                hasAirBag = car.HasAirBag,
                hasXenonLights = car.HasXenonLights,
                hasHalogenHeadlights = car.HasHalogenHeadlights,
                hasTractionControl = car.HasTractionControl,
                hasParktronic = car.HasParktronic,
                hasAlarm = car.HasAlarm,
                hasImobilizer = car.HasImobilizer,
                hasCentralLock = car.HasCentralLock,
                hasInsurance = car.HasInsurance,
                isArmored = car.IsArmored,
                isKeyless = car.IsKeyless,
                isTiptronicMultitronic = car.IsTiptronicMultitronic,
                hasAutopilot = car.HasAutopilot,
                hasPowerSteering = car.HasPowerSteering,
                hasOnboardComputer = car.HasOnboardComputer,
                hasServiceBook = car.HasServiceBook,
                hasWarranty = car.HasWarranty,
                hasNavigationSystem = car.HasNavigationSystem,
                isRightHandDrive = car.IsRightHandDrive,
                hasTuning = car.HasTuning,
                hasPanoramicRoof = car.HasPanoramicRoof,
                isTaxi = car.IsTaxi,
                isRetro = car.IsRetro,
                hasTow = car.HasTow,
                hasMoreSeats = car.HasMoreSeats,
                hasRefrigerator = car.HasRefrigerator,
                isApproved = car.IsApproved,
                isAdvert = car.IsAdvert,
                creatorId = 0,
                editorId = 0
            };

            Db.Mapper.Execute(sql, queryParams);
        }

        public static void Delete(uint id)
        {
            var sql = "DELETE FROM `car` WHERE id = @id";

            Db.Mapper.Query(sql, new { id });
        }
    }
}
