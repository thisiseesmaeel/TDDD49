using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Messenger.ViewModels;

namespace Messenger.Commands
{
    public class UpdateFirstName : ICommand
    {
        private UserViewModel _userViewModel { set; get; }


        #region ICommand Members  
        public UpdateFirstName(UserViewModel UserVM)
        {
            _userViewModel = UserVM;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _userViewModel.TestMethod();
        }
        #endregion
    }
}