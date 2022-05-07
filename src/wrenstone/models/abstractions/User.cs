using System.Text.Json.Serialization;
using wrenstone.converters;
using wrenstone.models.enums;
using wrenstone.models.interfaces;

namespace wrenstone.models.abstractions
{
    [JsonConverter(typeof(UserConverter))]
    public abstract class User : Entity<string>, IUser
    {
        [JsonConstructor]
        public User() { }

        public UserType UserType { get; set; }
    }
}
