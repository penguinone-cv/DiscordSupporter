using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSupporter.Utils
{
    public static class DiscordUtils
    {
        public static bool IsEmpty(this string txt)
        {
            return string.IsNullOrEmpty(txt) || string.IsNullOrWhiteSpace(txt);
        }
    }
}
