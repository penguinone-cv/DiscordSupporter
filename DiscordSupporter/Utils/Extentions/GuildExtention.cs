using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSupporter.Utils
{
    public static class GuildExtention
    {
    }

    public static class SocketGuildExtention
    {
        public static IRole? GetRole(this SocketGuild guild, string roleName)
        {
            return guild.Roles.FirstOrDefault(role => role.Name == roleName);
        }

        
    }
}
