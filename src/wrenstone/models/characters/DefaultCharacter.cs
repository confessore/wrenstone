using wrenstone.models.abstractions;

namespace wrenstone.models.characters
{
    public class DefaultCharacter : Character
    {
        public string? Name { get; set; }
        public int? FactionId { get; set; }
        public string? Race { get; set; }
        public string? Class { get; set; }
        public string? GuildName { get; set; }
        public int? Level { get; set; }

        public DateTimeOffset? Fetched { get; set; }
    }
}
