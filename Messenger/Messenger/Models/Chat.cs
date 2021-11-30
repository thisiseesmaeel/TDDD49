using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Models
{
    public class Chat
    {
        public Chat(string Txt, string sender,DateTime date)
        {
            Text = Txt;
            Sender = sender;
            Date = date;
        }
        public string Text { get; set; }
        public string Sender { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get { return Date.ToString("yyyy-MM-dd H:mm"); } }
    }
}
