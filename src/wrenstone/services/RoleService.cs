using Discord.WebSocket;
using System.Linq;

namespace wrenstone.services
{
    public class RoleService
    {
        public async Task UpdateClassRoleAsync(SocketGuild guild, SocketGuildUser user, string name)
        {
            foreach (var userRole in user.Roles.ToArray())
            {
                if (classes.Any(x => x == userRole.Name))
                    await user.RemoveRoleAsync(userRole);
            }
            var guildRole = FindGuildRole(guild, name);
            if (guildRole != null)
                await user.AddRoleAsync(guildRole);
        }

        public async Task UpdateFactionRoleAsync(SocketGuild guild, SocketGuildUser user, string name)
        {
            foreach (var userRole in user.Roles.ToArray())
            {
                if (factions.Any(x => x == userRole.Name))
                    await user.RemoveRoleAsync(userRole);
            }
            var guildRole = FindGuildRole(guild, name);
            if (guildRole != null)
                await user.AddRoleAsync(guildRole);
        }

        SocketRole? FindGuildRole(SocketGuild guild, string name) =>
            guild.Roles.Where(x => x.Name == name.ToLower()).FirstOrDefault();

        public string[] classes = new string[9]
        {
            "druid",
            "hunter",
            "mage",
            "paladin",
            "priest",
            "rogue",
            "shaman",
            "warlock",
            "warrior"
        };

        public string[] factions = new string[2]
        {
            "horde",
            "alliance"
        };
    }
}
