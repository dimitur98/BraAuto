using BraAutoDb.Models;
using BraAutoDb.Models.ReviewsSearch;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BraAuto.ViewModels
{
    public class ReviewSearchModel: BaseSearchModel<Response, Review>
    {
        public ReviewSearchModel()
        {
            this.SortFields = new List<(string Name, string SortColumn, bool SortDesc, bool Specific)> { ("Newest First", "r.created_at", true, false), ("Oldest First", "r.created_at", false, false), ("Rating High-Low", "r.star_rating", true, false), ("Rating Low-High", "r.star_rating", false, false) };
        }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("Search", "Reviews") };

            return new Breadcrumb(paths);
        }

        public Request ToSearchRequest()
        {
            var request = new Request();

            this.SetSearchRequest(request);

            return request;
        }
    }

    public class ReviewCreateModel
    {
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Rating")]
        public uint? StarRating { get; set; }
    }
}
