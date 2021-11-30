using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.ViewModels.Commands
{
    public class ChatCommand : ICommand
    {
        ChatViewModel _chatViewModel;
        public ChatCommand(ChatViewModel chatViewModel)
        {
            _chatViewModel = chatViewModel;
            BaseViewModel.UserModel.PropertyChanged += OnViewModelPropertyChanged;
            _chatViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {

            return !BaseViewModel.UserModel.ConnectionEnded && !string.IsNullOrEmpty(_chatViewModel.MessageToSend);
        }

        public void Execute(object parameter)
        {
            BaseViewModel.UserModel.Chat(_chatViewModel.MessageToSend);
            _chatViewModel.MessageToSend = "";
        }
  
        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if( e.PropertyName ==  "ConnectionEnded" || e.PropertyName == "MessageToSend")
                App.Current.Dispatcher.Invoke(() => { CanExecute(null); });
        }
    }
}
