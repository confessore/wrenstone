using wrenstone.models.interfaces;

namespace wrenstone.models.abstractions
{
    public abstract class Entity<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey? Id { get; set; }
    }
}