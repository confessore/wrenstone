using Discord.WebSocket;
using HtmlAgilityPack;
using Serilog;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using wrenstone.models.abstractions;
using wrenstone.models.characters;

namespace wrenstone.services
{
    public class ArmoryService
    {
        readonly HttpClient _httpClient;

        public ArmoryService(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async Task<string> GetStringFromUrlAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        async Task<XmlDocument> GetHtmlDocumentFromStringAsync(string url)
        {
            var tmp = new XmlDocument();
            var str = await GetStringFromUrlAsync(url);
            tmp.LoadXml(str);
            return tmp;
        }

        public async Task<DefaultCharacter> LookupCharacterAsync(string name)
        {
            var document = await GetHtmlDocumentFromStringAsync($"http://armory.twinstar.cz/character-sheet.xml?r=KronosIV&cn={name}");
            var character = document.DocumentElement.SelectSingleNode(@"//character");

            return new DefaultCharacter()
            {
                Name = character.Attributes["name"].Value,
                Level = int.TryParse(character.Attributes["level"].Value, out var level) ? level : int.MinValue,
                FactionId = int.TryParse(character.Attributes["factionId"].Value, out var factionId) ? factionId : int.MinValue,
                Class = character.Attributes["class"].Value,
                GuildName = character.Attributes["guildName"].Value,
                Race = character.Attributes["race"].Value,
                Fetched = DateTimeOffset.UtcNow
            };
            //Log.Information(character.Attributes["charUrl"].Value);
            //Log.Information(character.Attributes["guildUrl"].Value);
        }

        public string RecurseXPath(string element, int levels)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < levels; i++)
            {
                builder.Append(element);
                if (i < levels - 1)
                    builder.Append("//");
            }
            return builder.ToString();
        }

        //public async Task VerifyGuildMemberAsync(SocketGuild guild, SocketGuildUser user, string name)
        //{
        //    var role = await baseService.GetGuildRoleAsync(guild, "member");
        //    if (!user.Roles.Contains(role))
        //    {
        //        var members = await GetGuildMembersAsync();
        //        if (members.Any(x => x == name))
        //            await baseService.ModifyRoleAsync(guild, user, role.Name);
        //    }

        //}
    }
}
