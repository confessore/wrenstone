using wrenstone.models.abstractions;
using wrenstone.models.interfaces;

namespace wrenstone.models.users
{
    public class DefaultUser : User, IEntity<ulong>, IUser
    {
    }
}
