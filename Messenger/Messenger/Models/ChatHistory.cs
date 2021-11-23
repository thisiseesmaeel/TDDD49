using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class ChatHistory
    {
        public ChatHistory()
        {
            Name = "";
            ChatLog = new List<Chat>();
        }
        public string Name { get; set; }
        public List<Chat> ChatLog { get; set; }

    }
}
