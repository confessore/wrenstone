using wrenstone.models.interfaces;

namespace wrenstone.models.abstractions
{
    public abstract class Entity<TId> : IEntity<TId>
        where TId : IEquatable<TId>
    {
        public TId? Id { get; set; }
    }
}
