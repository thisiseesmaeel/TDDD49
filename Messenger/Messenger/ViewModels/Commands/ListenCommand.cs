using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Messenger.ViewModels.Commands
{
    public class ListenCommand : ICommand
    {
        private StartViewModel _userViewModel;

        public ListenCommand(StartViewModel UserVM)
        {
            _userViewModel = UserVM;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            BaseViewModel.UserModel.Listen();
        }
    }
}
