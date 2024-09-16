using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordSupporter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DiscordSupporter
{
    public class MessageSender
    {
        private readonly DiscordSocketClient _client;

        public MessageSender(DiscordSocketClient client)
        {
            _client = client;
        }

        /// <summary>
        /// メッセージ送信
        /// </summary>
        /// <param name="message"></param>
        /// <param name="channel"></param>
        /// <param name="channelMention"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string message, ISocketMessageChannel channel, bool channelMention = false)
        {
            if (channelMention)
            {
                message = $"<@&{channel.Name}> " + message;
            }
            await channel.SendMessageAsync(message);
        }

        /// <summary>
        /// リプライを飛ばす
        /// </summary>
        /// <param name="target"></param>
        /// <param name="message"></param>
        /// <param name="channel"></param>
        /// <param name="channelMention"></param>
        /// <returns></returns>
        public async Task ReplyMessageAsync(SocketUserMessage target, string message, ISocketMessageChannel channel, bool channelMention)
        {
            var context = new SocketCommandContext(_client, target);
            if (channelMention)
            {
                var guild = context.Guild;
                var role = guild.GetRole(channel.Name);
                if (role == null)
                {
                    return;
                }
                message = $"<@&{role.Id}> " + message;
            }
            await context.Message.ReplyAsync(message);
        }

        // テスト用
        public async Task ReplyMessageAsync(SocketUserMessage target, string message, ISocketMessageChannel channel, string roleName)
        {
            var context = new SocketCommandContext(_client, target);
            var guild = context.Guild;
            var role = guild.GetRole(roleName);
            if (role == null)
            {
                return;
            }
            message = $"<@&{role.Id}> " + message;
            await channel.SendMessageAsync(message);
        }
    }
}
