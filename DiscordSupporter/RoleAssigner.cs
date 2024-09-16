using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordSupporter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSupporter
{
    public partial class Entry
    {
        internal async Task ReactionAdded(Cacheable<IUserMessage, ulong> messageCacheable, Cacheable<IMessageChannel, ulong> channelCacheable, SocketReaction reaction)
        {
            var message = await messageCacheable.GetOrDownloadAsync();

            var guild = Client.GetGuild(_config.GuildId);
            if (message.Channel.IsGameCategory(guild))
            {
                // ユーザー取得
                var user = guild.GetUser(reaction.User.Value.Id);
                // チャンネル名のロール割り当て
                await RoleAssigner.AssignRole(guild, user, message.Channel.Name);
            }
        }

        internal async Task AssignRoleViaMessage(SocketMessage message)
        {
            var guild = Client.GetGuild(_config.GuildId);
            if (message.Channel.IsGameCategory(guild))
            {
                var author = guild.GetUser(message.Author.Id);
                await RoleAssigner.AssignRole(guild, author, message.Channel.Name);
            }
            
            
        }
    }
    public class RoleAssigner
    {
        private readonly DiscordSocketClient _client;

        public RoleAssigner(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task AssignRole(SocketGuild guild, SocketGuildUser target, string roleName)
        {
            var role = guild.Roles.FirstOrDefault(role => role.Name == roleName);
            ulong roleId = 0;
            // ロールが見つからなければ追加
            if (role == null)
            {
                var newRole = await guild.CreateRoleAsync(roleName, isMentionable: true);
                roleId = newRole.Id;
            }
            else
            {
                roleId = role.Id;
            }

            await target.AddRoleAsync(roleId);
        }
    }
}
