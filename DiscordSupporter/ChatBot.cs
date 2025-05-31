using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSupporter
{
    public class ChatBot
    {
        public ChatBot(ChatClient client, string ragDataPath)
        {
            Client = client;
            RAGDataPath = ragDataPath;
        }

        public ChatClient Client { get; init; }

        private DateTime LastRAGDataLoadTime { get; set; } = DateTime.MinValue;

        private string RAGDataPath { get; init; }

        private string RAGData { get; set; }

        private TimeSpan RAGLoadThres { get; set; } = new TimeSpan(1, 0, 0, 0);

        public string GetRAGData()
        {
            var dateTime = DateTime.Now;
            if (dateTime - LastRAGDataLoadTime > RAGLoadThres)
            {
                UpdateRAGData();
                LastRAGDataLoadTime = dateTime;
            }
            return RAGData;
        }

        private void UpdateRAGData()
        {
            using StreamReader sr = new StreamReader(RAGDataPath);
            RAGData = sr.ReadToEnd();
        }
    }
}
