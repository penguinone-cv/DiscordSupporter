using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordSupporter
{
    public partial class Entry
    {
        /// <summary>
        /// SocketMessageから処理を分岐するための拡張メソッド
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        internal async Task MessageReceived(SocketMessage message)
        {
            // 自分のメッセージは無視
            if (Analyzer.IsOwnMessage(message))
            {
                return;
            }

            // メンションされたら返事
            if (Analyzer.IsMentioned(message))
            {
                await Sender.SendMessageAsync("はーい", message.Channel);
            }

            // 募集メッセージに対するリプライ
            if (Analyzer.IsCollectMemberMessage(message))
            {
                var txt = "募集らしきメッセージが送られていそうですよ？";
                await Sender.ReplyMessageAsync(message as SocketUserMessage, txt, message.Channel, true);
            }

            await AssignRoleViaMessage(message);
        }
    }

    /// <summary>
    /// 構文解析
    /// </summary>
    public class MessageAnalyzer
    {
        private readonly DiscordSocketClient _client;

        public MessageAnalyzer(DiscordSocketClient client)
        {
            _client = client;
        }

        public bool IsOwnMessage(SocketMessage message)
        {
            return message.Author.Id == _client.CurrentUser.Id;
        }

        public bool IsMentioned(SocketMessage message)
        {
            var userMentioned = message.MentionedUsers.Any(user => user.Id == _client.CurrentUser.Id);
            var roleMentioned = message.MentionedRoles.Any(role => role.Members.Any(member => member.Id == _client.CurrentUser.Id));
            return userMentioned || roleMentioned;
        }

        public bool IsCollectMemberMessage(SocketMessage message)
        {
            var content = message.Content;
            bool isCollectMessage = content.Contains("募集");
            bool isNotMentioned = message.MentionedChannels.Count == 0 && !message.MentionedEveryone && message.MentionedRoles.Count == 0 && message.MentionedUsers.Count == 0;
            bool isAtMessage = isNotMentioned && Regex.IsMatch(content, "@[0-9]");
            return isCollectMessage || isAtMessage;
        }

        public bool IsMemberMessage(SocketMessage message)
        {
            return !IsOwnMessage(message) && !message.Author.IsBot;
        }
    }
}
