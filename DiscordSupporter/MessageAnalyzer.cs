using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenAI.Chat;

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
            if (await Analyzer.IsCollectMemberMessage(message))
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
        private readonly ChatClient _chat;

        public MessageAnalyzer(DiscordSocketClient client, ChatClient chatClient)
        {
            _client = client;
            _chat = chatClient;
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

        public async Task<bool> IsCollectMemberMessage(SocketMessage message)
        {
            var content = message.Content;

            // ChatGPTくんに聞いてみよう
            // System Prompt & Few-shot
            var systemTxt = "あなたは、「提示されたメッセージがメンバーを募集するメッセージであるか」を判断するスペシャリストです。"
                          + "始めに理由を述べ、最後に判断を「Yes」か「No」で答えてください。"
                          + "例えば、「21- @2」というメッセージの場合、「21-」が21時から、「@2」が残り募集人数が2人であることを表すため、21時から2人を募集するメッセージと解釈でき、「Yes」です。"
                          + "また、「soloってます。」というメッセージの場合、一人でゲームをしており、誰か暇な人がいれば一緒にやりたいことを暗に示しているため、「Yes」です。"
                          + "一方で、「(エペ募集の方に僕入ってますが工場は開けれるのでご心配なく)」というメッセージは「募集」という言葉は入っていますが、メッセージの主題は募集することではなく「工場が開けられること」にあるため、「No」です。\n";
            var askTxt = $"以下のメッセージの場合はどうなりますか？\n"
                       + $"{content}";
            var result = await _chat.CompleteChatAsync(systemTxt+askTxt);
            if (result.Value.Content[0].Text.Contains("Yes"))
            {
                Console.WriteLine($"ChatGPTによる推論が利用されました。Model：{result.Value.Model}");
                return true;
            }
            return false;

            //bool isCollectMessage = content.Contains("募集");
            //bool isMentioned = message.MentionedChannels.Count > 0 || message.MentionedEveryone || message.MentionedRoles.Count > 0 || message.MentionedUsers.Count > 0;
            //bool isAtMessage = !isMentioned && Regex.IsMatch(content, "@[0-9]");
            //if (isCollectMessage && !isMentioned)
            //{
            //    return true;
            //}
            //return isCollectMessage || isAtMessage;
        }

        public bool IsMemberMessage(SocketMessage message)
        {
            return !IsOwnMessage(message) && !message.Author.IsBot;
        }
    }
}
