using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordSupporter.Utils;

namespace DiscordSupporter
{
    public class SlashCommand
    {
        private readonly DiscordSocketClient _client;
        private readonly ulong _guildId;
        private SocketGuild _guild;

        private List<SlashCommandBuilder> SlashCommands { get; } = new List<SlashCommandBuilder>();

        public SlashCommand(DiscordSocketClient client, ulong guildId)
        {
            _client = client;
            _guildId = guildId;

            var voteCommand = new SlashCommandBuilder()
                .WithName("vote")
                .WithDescription("投票機能")
                .AddOption("title", ApplicationCommandOptionType.String, "投票タイトル", isRequired:true)
                .AddOption("vote_period", ApplicationCommandOptionType.Integer, "投票受付期間 時間単位で指定してください(1時間～)、指定なしで24時間", isRequired:false)
                .AddOption("allow_multi_select", ApplicationCommandOptionType.Boolean, "複数選択を許可するか", isRequired:false)
                .AddOption("candidate", ApplicationCommandOptionType.String, "候補 例：りんご バナナ みかん", isRequired: true);
            SlashCommands.Add(voteCommand);

            _client.SlashCommandExecuted += SlashCommandHandler;
        }

        public async void CreateAllCommands()
        {
            _guild = _client.GetGuild(_guildId);
            foreach (var command in SlashCommands)
            {
                await _guild.CreateApplicationCommandAsync(command.Build());
            }
        }

        public async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.CommandName)
            {
                case "vote":
                    await VoteCommand(command);
                    break;
                default:
                    await command.RespondAsync($"(このメッセージが表示されている場合は開発者に連絡してください)\nThis command is not implemented: {command.CommandName}", ephemeral:true);
                    break;
            }
        }

        #region スラッシュコマンド
        public async Task VoteCommand(SocketSlashCommand command)
        {
            // パラメータ解析
            var candParam = command.Data.Options.First(option => option.Name == "candidate").Value as string;
            if (candParam.IsEmpty())
            {
                await command.RespondAsync("候補が入力されていません。1つ以上の候補を入力してください。", ephemeral:true);
            }
            candParam = candParam.TrimStart().TrimEnd();
            var candidates = candParam.Split(" ");

            var poll = new PollProperties();
            var question = new PollMediaProperties();
            question.Text = command.Data.Options.First(option => option.Name == "title").Value as string;
            var answers = new List<PollMediaProperties>(candidates.Length);
            foreach (var candidate in candidates)
            {
                var ans = new PollMediaProperties();
                ans.Text = candidate;
                answers.Add(ans);
            }

            poll.Question = question;
            poll.Answers = answers;
            var allowMultiSelect = command.Data.Options.First(option => option.Name == "allow_multi_select").Value;
            poll.AllowMultiselect = allowMultiSelect != null ? (bool)allowMultiSelect : true;
            var duration = command.Data.Options.First(option => option.Name == "vote_period").Value as string;
            poll.Duration = !duration.IsEmpty() ? uint.Parse(duration) : 24;
            poll.LayoutType = PollLayout.Default;

            await command.RespondAsync(poll:poll);
        }
        #endregion
    }
}
