using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.ViewModels.Commands
{
    public class BUZZCommand : ICommand
    {
        ChatViewModel _chatViewModel;

        public BUZZCommand(ChatViewModel chatViewModel)
        {
            _chatViewModel = chatViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            BaseViewModel.UserModel.BUZZ();
;       }
    }
}
