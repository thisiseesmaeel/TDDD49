﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.ViewModels.Commands
{
    public class ConnectCommand : ICommand
    {
        private UserViewModel _userViewModel;

        #region ICommand Members  
        public ConnectCommand(UserViewModel UserVM)
        {
            _userViewModel = UserVM;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _userViewModel.UserModel.Connect(server: _userViewModel.UserModel.IP);
        }
        #endregion
    }
}