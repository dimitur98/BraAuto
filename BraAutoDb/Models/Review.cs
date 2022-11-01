using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "review")]
    public class Review : BaseModel<uint>
    {
        [Column(Name = "first_name")]
        public string FirstName { get; set; }

        [Column(Name = "last_name")]
        public string LastName { get; set; }

        [Column(Name = "description")]
        public string Description { get; set; }

        [Column(Name = "star_rating")]
        public uint StarRating { get; set; }

        [Column(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
