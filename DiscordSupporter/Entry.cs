using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using DiscordSupporter.Utils;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DiscordSupporter
{
    public partial class Entry
    {
        private readonly Config _config;
        internal DiscordSocketClient Client { get; private set; }
        private MessageAnalyzer Analyzer { get; }
        private MessageSender Sender { get; }
        private RoleAssigner RoleAssigner { get; }

        public Entry()
        {
            //var configData = FileReader.ReadFile("./config.json");
            //_config = JsonConvert.DeserializeObject<Config>(configData);
            _config = new Config();

            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                GatewayIntents = GatewayIntents.MessageContent | GatewayIntents.GuildMessages | GatewayIntents.AllUnprivileged
            });
            Client.Log += ClientLogAsync;
            Client.Ready += ClientReadyAsync;
            Client.MessageReceived += MessageReceived;
            Client.ReactionAdded += ReactionAdded;

            Analyzer = new MessageAnalyzer(Client);
            Sender = new MessageSender(Client);
            RoleAssigner = new RoleAssigner(Client);
        }

        public static void Main() => new Entry().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            await Client.LoginAsync(TokenType.Bot, _config.Token);
            Console.WriteLine("Logged in");
            await Client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private static async Task ClientLogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
        }

        private async Task ClientReadyAsync()
        {
            Console.WriteLine($"{Client.CurrentUser} is connected.");
        }
    }
}