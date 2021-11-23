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

        public delegate void SwitchToStartHandler();
        public event SwitchToStartHandler UserIntendsToGoBackEvent;

        public void RaiseUserIntendsToGoBackEvent()
        {
            UserModel.TearDownConnection();
            UserIntendsToGoBackEvent();
        }
    }
}
