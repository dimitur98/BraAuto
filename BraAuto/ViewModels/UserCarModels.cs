using BraAuto.ViewModels.Common;
using BraAutoDb.Models;
using BraAutoDb.Models.UserCarsSearch;
using System.ComponentModel;

namespace BraAuto.ViewModels
{
    public abstract class UserCarSearchBaseModel : BaseSearchModel<Response, UserCar>
    {
        public IEnumerable<uint> UserCarTypeIds { get; set; }

        public IEnumerable<uint> CarIds { get; set; }

        public uint? UserId { get; set; }

        public DateTime? Date { get; set; }

        public uint? CreatorId { get; set; }

        public Request ToSearchRequest()
        {
            var request = new Request
            {
                UserCarTypeIds = this.UserCarTypeIds,
                CarIds = this.CarIds,
                UserId = this.UserId,
                Date = this.Date,
                CreatorId = this.CreatorId
            };

            this.SetSearchRequest(request);

            return request;
        }
    }

    public class MyUserCarServiceModel : UserCarSearchBaseModel
    {
        public MyUserCarServiceModel()
        {
            this.SortFields = new List<(string Name, string SortColumn, bool SortDesc, bool Specific)> { ("Newest First", "uc.date", true, false), ("Oldest First", "uc.date", false, false) };
        }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("MyService", "UserCars") };

            return new Breadcrumb(paths, totalRecords: this.Response.TotalRecords);
        }
    }


    public class FavouriteCarModel : UserCarSearchBaseModel
    {
        public FavouriteCarModel()
        {
            this.SortFields = new List<(string Name, string SortColumn, bool SortDesc, bool Specific)> { ("Newest First", "c.created_at", true, false), ("Oldest First", "c.created_at", false, false) };
        }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("Favourite", "Cars") };

            return new Breadcrumb(paths, totalRecords: this.Response.TotalRecords);
        }
    }


    public class UserServiceBookAppointmentModel
    {
        [DisplayName("Car")]
        public uint CarId { get; set; }

        public IEnumerable<SimpleModel> Cars { get; set; }

        [DisplayName("Service")]
        public uint ServiceId { get; set; }

        public IEnumerable<SimpleModel> Services { get; set; }

        public uint Hour { get; set; }

        [DisplayName("Date")]
        public DateTime Date { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Search", "Users"), ("BookAppointment", "Users") };

            return new Breadcrumb(paths);
        }
    }
}

