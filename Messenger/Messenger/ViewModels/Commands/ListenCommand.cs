﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Messenger.ViewModels.Commands
{
    public class ListenCommand : ICommand
    {
        private StartViewModel _startViewModel;

        public ListenCommand(StartViewModel startViewModel)
        {
            _startViewModel = startViewModel;
            _startViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

       

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {

            return !string.IsNullOrEmpty(_startViewModel.DisplayName) && !string.IsNullOrEmpty(_startViewModel.IP)
                && !string.IsNullOrEmpty(_startViewModel.Port) && _startViewModel.ErrorMessage != "Listening..."
                && _startViewModel.ErrorMessage != "Connecting...";
        }

        public void Execute(object parameter)
        {
            if (!Regex.Match(_startViewModel.DisplayName, "^[a-zA-ZäöåÄÖÅ]+$").Success)
            {
                _startViewModel.ErrorMessage = "Invalid characters are not allowed in displayname (only alphabet including äöå)";
            }
            else if (!Regex.Match(_startViewModel.Port, @"\d").Success)
            {
                _startViewModel.ErrorMessage = "Invalid port number! Enter an integer.";
            }
            else if(!Regex.Match(_startViewModel.IP, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}").Success)
            {
                _startViewModel.ErrorMessage = "Invalid IP!";
            }
            else
            {
                _startViewModel.ErrorMessage = "Listening...";
                CanExecute(null);
                BaseViewModel.UserModel.Listen();
            }
            
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DisplayName" || e.PropertyName == "IP" || e.PropertyName == "Port")
                App.Current.Dispatcher.Invoke(() => { CanExecute(null); });
        }
    }
}
