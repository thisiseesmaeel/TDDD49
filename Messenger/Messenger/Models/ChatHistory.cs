using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class ChatHistory
    {
        public ChatHistory(DateTime date)
        {
            ChatLog = new List<Chat>();
            Date = date;
        }
        public List<Chat> ChatLog { get; set; }
        public DateTime Date { get; set; }

    }
}
