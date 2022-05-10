using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Serilog;
using wrenstone.contexts;
using wrenstone.services;

namespace wrenstone.modules
{
    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        readonly IServiceProvider _services;
        readonly DiscordSocketClient _client;
        readonly CommandService _commands;
        readonly RoleService _role;
        readonly ArmoryService _armory;
        readonly IDbContextFactory<DefaultDbContext> _defaultDbContextFactory;

        public CommandModule(
            IServiceProvider services,
            DiscordSocketClient client,
            CommandService commands,
            RoleService role,
            ArmoryService armory,
            IDbContextFactory<DefaultDbContext> defaultDbContextFactory)
        {
            _services = services;
            _client = client;
            _commands = commands;
            _role = role;
            _armory = armory;
            _defaultDbContextFactory = defaultDbContextFactory;
        }

        readonly Random random = new Random();

        [Command("help", RunMode = RunMode.Async)]
        [Summary("all: displays available commands" +
            "\n >help")]
        async Task HelpAsync()
        {
            await RemoveCommandMessageAsync();
            var embedBuilder = new EmbedBuilder();
            foreach (var command in await _commands.GetExecutableCommandsAsync(Context, _services))
                embedBuilder.AddField(command.Name, command.Summary ?? "no summary available");
            await ReplyAsync("here's a list of commands and their summaries: ", false, embedBuilder.Build());
        }

        [Command("insult", RunMode = RunMode.Async)]
        [Summary("all: got 'em" +
            "\n >insult")]
        async Task InsultAsync()
        {
            await RemoveCommandMessageAsync();
            await ReplyAsync("your mother");
        }

        [Command("nick", RunMode = RunMode.Async)]
        [Summary("all: change your nick" +
            "\n >nick 'your nick here'")]
        async Task NickAsync([Remainder] string name)
        {
            await RemoveCommandMessageAsync();
            await _client.GetGuild(Context.Guild.Id).GetUser(Context.User.Id).ModifyAsync(x => x.Nickname = name);
        }

        [Command("verify", RunMode = RunMode.Async)]
        [Summary("all: updates a single guild user's membership role according to their nickname (character name)" +
            "\n >verify" +
            "\n >verify")]
        async Task VerifyAsync()
        {
            await RemoveCommandMessageAsync();
            var user = _client.GetGuild(Context.Guild.Id).GetUser(Context.User.Id);
            var character = await _armory.LookupCharacterAsync(user.Nickname ?? user.Username);
            await _role.UpdateFactionRoleAsync(Context.Guild, (SocketGuildUser)Context.User, character.FactionId is 1 ? "horde" : "alliance");
            await _role.UpdateClassRoleAsync(Context.Guild, (SocketGuildUser)Context.User, character.Class ?? string.Empty);
            using var context = await _defaultDbContextFactory.CreateDbContextAsync();
            if (context.Users != null)
            {

            }
            Log.Information($"{(character.FactionId is 1 ? "horde" : "alliance")} role was added for {character.Name.ToLower()}");
            Log.Information($"{character.Class.ToLower()} role was added for {character.Name.ToLower()}");
            //await ReplyAsync($"{character.Name} is a {character.Level} {character.Race} {character.Class} in the guild {character.GuildName}");
            //await htmlService.VerifyGuildMemberAsync(Context.Guild, user, user.Nickname ?? user.Username);
        }

        async Task RemoveCommandMessageAsync() =>
            await _client.GetGuild(Context.Guild.Id).GetTextChannel(Context.Message.Channel.Id).DeleteMessageAsync(Context.Message);
    }
}
