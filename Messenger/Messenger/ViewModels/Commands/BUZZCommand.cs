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
        public BUZZCommand(ChatViewModel chatViewModel)
        {
           
            BaseViewModel.UserModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public bool CanExecute(object parameter)
        {
            return !BaseViewModel.UserModel.ConnectionEnded;
        }

        public void Execute(object parameter)
        {
            BaseViewModel.UserModel.BUZZ();
;       }


        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Buzz")
                App.Current.Dispatcher.Invoke(() => { CanExecute(null); });
        }
    }
}
