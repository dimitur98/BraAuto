using BraAutoDb.Dal;
using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "department")]
    public class Article : AuditInfo<uint>
    {
        [Column(Name = "title")]
        public string Title { get; set; }

        [Column(Name = "body")]
        public string Body { get; set; }

        [Column(Name = "category_id")]
        public uint CategoryId { get; set; }

        [Column(Name = "photo_url")]
        public string PhotoUrl { get; set; }

        public Category Category { get; set; }

        [Column(Name = "is_approved")]
        public bool IsApproved { get; set; }

        public void LoadCategory()
        {
            this.Category = Db.Categories.GetById(this.CategoryId);
        }
    }
}
