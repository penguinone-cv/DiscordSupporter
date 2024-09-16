using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSupporter.Utils
{
    public static class ChannelExtention
    {
        public static bool IsGameCategory(this IMessageChannel channel, SocketGuild guild)
        {
            var categories = guild.CategoryChannels;
            foreach (var category in categories)
            {
                if (category.Channels.Any(ch => ch.Id == channel.Id))
                {
                    if (category.Name == "ゲームチャンネル")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
