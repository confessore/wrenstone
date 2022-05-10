using System.Text.Json.Serialization;
using wrenstone.converters;
using wrenstone.models.enums;
using wrenstone.models.interfaces;

namespace wrenstone.models.abstractions
{
    [JsonConverter(typeof(CharacterConverter))]
    public abstract class Character : Entity<ulong>, IEntity<ulong>, ICharacter
    {
        [JsonConstructor]
        public Character() { }

        public CharacterType CharacterType { get; set; }
    }
}
