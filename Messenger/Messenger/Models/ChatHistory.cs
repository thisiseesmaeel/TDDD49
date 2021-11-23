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
            
            ChatLog = new List<Chat>();
        }
        public List<Chat> ChatLog { get; set; }

    }
}
