using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSupporter
{
    [JsonObject("Config")]
    public class Config
    {
        [JsonProperty("Token")]
        public string Token { get; set; } = "Dummy_Token";

        /// <summary>
        /// 一旦特定のサーバー専用にする
        /// </summary>
        [JsonProperty("GuildId")]
        public ulong GuildId { get; set; } = 0;
    }
}
