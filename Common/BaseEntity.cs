using System;

namespace VpniFy.Backend.Common
{
    public interface IEntity
    {
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }

    public abstract class BaseEntity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
        }
    }

    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
