﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Messenger.Models;
using System.Windows.Input;
using Messenger.ViewModels.Commands;
using System.Windows;
using System.Collections.ObjectModel;

namespace Messenger.ViewModels
{
    public class StartViewModel: BaseViewModel, INotifyPropertyChanged
    {
        public StartViewModel()
        {
            _listenCommand = new ListenCommand(this);
            _connectCommand = new ConnectCommand(this);
            _showHistoryCommand = new ShowHistoryCommand(this);
            _searchCommand = new SearchCommand();

            if(BaseViewModel.UserModel == null)
            {
                BaseViewModel.UserModel = new User();
            }
            BaseViewModel.UserModel.PropertyChanged += MyViewModelPropertyChanged;
            DisplayName = UserModel.DisplayName;
            UserModel.LoadHistory();
            ErrorMessage = "";
        }

        #region Fields
        public delegate void SwitchToChatHandler();
        public event SwitchToChatHandler UserIntendsToChatEvent;

        public delegate void SwitchToHistoryHandler(ChatHistory chatHistoryObj);
        public event SwitchToHistoryHandler UserIntendsToViewHistoryEvent;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public String DisplayName
        {
            get { return UserModel.DisplayName; }
            set { UserModel.DisplayName = value; OnPropertyChanged("DisplayName"); }
        }
        
        public String IP
        {
            get { return UserModel.IP; }
            set { UserModel.IP = value; OnPropertyChanged("IP"); }
        }
        public string Port
        {
            get { return UserModel.Port; }
            set { UserModel.Port = value; OnPropertyChanged("Port"); }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }


        public List<ChatHistory> ChatHistory
        { 
            get { return UserModel.ChatHistoryResultList; }
            //set { UserModel.ChatHistoryResultList = value ; OnPropertyChanged("ChatHistory"); }
        }
        #endregion

        private void OnPropertyChanged(String PropertyName)
        {
            if(this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        private void MyViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName) 
            {
                case "ShowInvitationMessageBox":
                    if(UserModel.ShowInvitationMessageBox)
                        ShowInvitationMessageBox();
                    break;
                case "ShowSocketExceptionMessageBox":
                    ShowSocketExceptionMessageBox();
                    break;
                case "ResponseToRequest":
                    ShowResponseToRequestMessageBox();
                    break;
                case "ChatHistoryResultList":
                    Console.WriteLine("ChatHistory Property changed!");
                    OnPropertyChanged("ChatHistory");
                    break;
                default:
                    break;
            }

        }

        private void ShowInvitationMessageBox()
        {
            // Configure the message box to be displayed
            string messageBoxText = $"Dear {DisplayName} do you want to chat with {UserModel.Chatpartner}?"; //Name of the person should be added as well later.
            string caption = "Permission";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            //display message box and save the result
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            
            // Process message box results
            switch (result)
            {
                case MessageBoxResult.Yes:
                    UserModel.AcceptRequest = true;
                    // should navigate to a new ViewModel
                    BaseViewModel.UserModel.PropertyChanged -= MyViewModelPropertyChanged;
                    UserIntendsToChatEvent();

                    break;
                case MessageBoxResult.No:
                    UserModel.AcceptRequest = false;
                    ErrorMessage = "";
                    break;
                default:
                    UserModel.AcceptRequest = false;
                    break;
            }
            
        }

        private void ShowSocketExceptionMessageBox()
        {
            // Configure the message box to be displayed
            string messageBoxText = "There is no one listening on this IP and port!";
            string caption = "Warning";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            if (result == MessageBoxResult.OK)
                ErrorMessage = "";
        }
        
        private void ShowResponseToRequestMessageBox()
        {
            // Configure the message box to be displayed
            string messageBoxText = "Your request is accepted.";
            bool Accepted = true;
            if (!UserModel.ResponseToRequest)
            {
                messageBoxText = "Your request is denied.";
                Accepted = false;
            }
            string caption = "Information";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            // Process message box results
            if(result == MessageBoxResult.OK)
            {
                // should navigate to a new ViewModel
                if (Accepted)
                {
                    BaseViewModel.UserModel.PropertyChanged -= MyViewModelPropertyChanged;
                    UserIntendsToChatEvent();
                }
                else
                {
                    ErrorMessage = "";
                }

            }
               
            
        }

        public void RaiseUserIntendsToViewHistoryEvent(ChatHistory chatHistoryObj)
        {
            BaseViewModel.UserModel.PropertyChanged -= MyViewModelPropertyChanged;
            UserIntendsToViewHistoryEvent(chatHistoryObj);
        }

        #region Commands
        private ListenCommand _listenCommand;
        public ICommand ListenCommand => _listenCommand;

        private ConnectCommand _connectCommand;
        public ICommand ConnectCommand => _connectCommand;

        public ShowHistoryCommand _showHistoryCommand;
        public ICommand ShowHistoryCommand => _showHistoryCommand;

        public SearchCommand _searchCommand;
        public ICommand SearchCommand => _searchCommand;

        #endregion
    }
}
