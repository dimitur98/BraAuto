using BraAutoDb.Models;
using System.ComponentModel;

namespace BraAuto.ViewModels
{
    public class CarSearchModel : BaseSearchModel
    {
        [DisplayName("Year")]
        public IEnumerable<uint> YearIds { get; set; }

        [DisplayName("Body Type")]
        public IEnumerable<uint> BodyTypeIds { get; set; }

        [DisplayName("Make")]
        public uint MakeId { get; set; }
        public IEnumerable<Make> Makes { get; set; }

        [DisplayName("Model")]
        public IEnumerable<uint> ModelIds { get; set; }

        [DisplayName("Fuel")]
        public IEnumerable<uint> FuelTypeIds { get; set; }

        [DisplayName("Gearbox")]
        public IEnumerable<uint> GearboxTypeIds { get; set; }
    }

    public class CarMainSearchModel : CarSearchModel { }
}
