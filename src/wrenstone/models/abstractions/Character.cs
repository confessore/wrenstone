using wrenstone.models.enums;
using wrenstone.models.interfaces;

namespace wrenstone.models.abstractions
{
    public class Character : Entity<string>, ICharacter
    {
        public CharacterType CharacterType { get; set; }
    }
}
