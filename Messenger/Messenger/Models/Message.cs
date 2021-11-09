using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class Message
    {
        public Message(string requesttype, string sender, DateTime date, string messageText)
        {
            RequestType = requesttype;
            Sender = sender;
            Date = date;
            MessageText = messageText;

        }
        public string RequestType { get; set; }

        public string Sender { get; set; }

        public DateTime Date { get; set; }

        public string MessageText { get; set; }

    }
}
