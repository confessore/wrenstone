using System.Text.Json.Serialization;
using wrenstone.converters;
using wrenstone.models.enums;
using wrenstone.models.interfaces;

namespace wrenstone.models.abstractions
{
    [JsonConverter(typeof(CharacterConverter))]
    public abstract class User : Entity<ulong>, IEntity<ulong>, IUser
    {
        [JsonConstructor]
        public User() { }

        public UserType UserType { get; set; }
        public virtual Character? Character { get; set; }
        public virtual int? Chits { get; set; }
    }
}
