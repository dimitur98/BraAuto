using BraAutoDb.Models;
using BraAutoDb.Models.ArticlesSearch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BraAuto.ViewModels
{
    public abstract class ArticleSearchBaseModel : BaseSearchModel<Response, Article>
    {
        public ArticleSearchBaseModel()
        {
            this.SortFields = new List<(string Name, string SortColumn, bool SortDesc, bool Specific)> { ("Newest First", "a.created_at", true, false), ("Oldest First", "a.created_at", false, false), ("Approved First", "a.is_approved", true, true), ("Not Approved First", "a.is_approved", false, true) };
        }

        public string Keywords { get; set; }

        public uint? CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; }


        public bool? IsApproved { get; set; }

        public Request ToSearchRequest()
        {
            var request = new Request
            {
                Keywords = this.Keywords,
                CategoryId = this.CategoryId,
                IsApproved = this.IsApproved
            };

            this.SetSearchRequest(request);

            return request;
        }
    }

    public class ArticleSearchModel : ArticleSearchBaseModel
    {
        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("Search", "Articles") };

            return new Breadcrumb(paths);
        }
    }

    public class MyArticleModel : ArticleSearchBaseModel
    {
        [DisplayName("User")]
        public IEnumerable<uint> UserIds { get; set; }

        public new Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("My Articles", "Articles") };

            return new Breadcrumb(paths);
        }
    }

    public class ArticleAdminModel : ArticleSearchBaseModel
    {
        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Home", "Cars"), ("Admin", "Articles") };

            return new Breadcrumb(paths);
        }
    }

    public class ArticleBaseModel
    {
        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Body")]
        public string Body { get; set; }

        [Required]
        [DisplayName("Category")]
        public uint CategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }

    public class ArticleCreateModel : ArticleBaseModel 
    {

        [Required]
        public IFormFile Photo { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Search", "Articles"), ("Create", "Articles") };

            return new Breadcrumb(paths);
        }
    }

    public class ArticleEditModel : ArticleBaseModel
    {
        [Key]
        [Required]
        [HiddenInput]
        public uint Id { get; set; }

        public IFormFile Photo { get; set; }

        public string PhotoUrl { get; set; }

        public Breadcrumb ToBreadcrumb()
        {
            var paths = new List<(string Action, string Controller)>() { ("Search", "Articles"), ("Create", "Articles") };

            return new Breadcrumb(paths);
        }
    }
    
    public class ArticleDetailsModel
    {
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Body")]
        public string Body { get; set; }

        [DisplayName("Category")]
        public Category Category { get; set; }

        public string PhotoUrl { get; set; }

        [DisplayName("Creator")]
        public User Creator { get; set; }

        [DisplayName("Created At")]
        public DateTime CreatedAt { get; set; }
    }
}
