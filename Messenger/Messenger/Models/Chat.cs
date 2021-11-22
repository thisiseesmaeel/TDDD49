using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class Chat
    {
        public Chat(string Txt, string sender)
        {
            Text = Txt;
            Sender = sender;
        }
        public string Text { get; set; }
        public string Sender { get; set; }
    }
}
