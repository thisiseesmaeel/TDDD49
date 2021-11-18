using Messenger.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.ViewModels
{
    public class BaseViewModel
    {
        public static User UserModel { set; get; }

        public delegate void SwitchToChatHandler();
        public delegate void SwitchToMainHandler();
        public event SwitchToChatHandler UserIntendsToChatEvent;
        public event SwitchToMainHandler UserIntendsToGoBack;

     

        public void RaiseUserIntendsToChatEvent()
        {
            UserIntendsToChatEvent();
        }

        public void RaiseUserIntendsToGoBack()
        {
            UserIntendsToGoBack();
        }
    }
}
