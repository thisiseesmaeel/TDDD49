using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.ViewModels.Commands
{
    public class BackToMainCommand : ICommand
    {
        private ChatViewModel _chatViewModel;
        public event EventHandler CanExecuteChanged;

        public BackToMainCommand(ChatViewModel chatViewModel)
        {
            _chatViewModel = chatViewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _chatViewModel.RaiseBackEvent();
        }
    }
}
