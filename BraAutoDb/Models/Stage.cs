
using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "stage")]
    public class Stage : BaseModel<uint>
    {
        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "sort_order")]
        public int SortOrder { get; set; }
    }
}
