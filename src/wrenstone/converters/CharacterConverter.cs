using System.Text.Json;
using System.Text.Json.Serialization;
using wrenstone.models.abstractions;
using wrenstone.models.characters;
using wrenstone.models.enums;

namespace wrenstone.converters
{
    public sealed class CharacterConverter : JsonConverter<Character>
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeof(Character).IsAssignableFrom(typeToConvert);

        public override Character? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var clone = reader;
            while (clone.Read())
            {
                if (clone.TokenType == JsonTokenType.PropertyName)
                {
                    var name = clone.GetString();
                    if (name != null)
                    {
                        if (name.ToLower() == nameof(CharacterType).ToLower())
                            break;
                    }
                }
            }
            clone.Read();
            var type = (CharacterType)clone.GetInt32();
            return type switch
            {
                CharacterType.Default => JsonSerializer.Deserialize<DefaultCharacter>(ref reader, options),
                //UserType.Vendor => JsonSerializer.Deserialize<VendorUser>(ref reader, options),
                _ => null,
            };
        }

        public override void Write(Utf8JsonWriter writer, Character value, JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
