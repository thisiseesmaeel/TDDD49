using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class ChatHistory
    {
        public ChatHistory(string name)
        {
            Name = name;
            ChatLog = new List<Chat>();
            ChatLog.Add(new Chat("Hello!", "Me"));
            ChatLog.Add(new Chat("Hi there!", "Ismail"));
        }
        public string Name { get; set; }
        public List<Chat> ChatLog { get; set; }

    }
}
