using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    public abstract class BaseModel<TKey>
    {
        [Column(Name = "id")]
        public TKey Id { get; set; }
    }
}
