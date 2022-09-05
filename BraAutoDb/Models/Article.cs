using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    [Table(Name = "department")]
    public class Article : AuditInfo<uint>
    {
        public string Title { get; set; }

        public string Body { get; set; }
    }
}
