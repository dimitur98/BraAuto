using DapperMySqlMapper;

namespace BraAutoDb.Models
{
    public abstract class AuditInfo<TKey> : BaseModel<TKey>
    {
        [Column(Name = "creator_id")]
        public uint CreatorId { get; set; }
        public User Creator { get; set; }

        [Column(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [Column(Name = "editor_id")]
        public uint? EditorId { get; set; }
        public User Editor { get; set; }

        [Column(Name = "edited_at")]
        public DateTime? EditedAt { get; set; }
    }
}
