using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Messenger.Models;
using System.Windows.Input;
using Messenger.ViewModels.Commands;
using System.Windows;

namespace Messenger.ViewModels
{
    public class UserViewModel: INotifyPropertyChanged
    {
        private User _userModel;
        public User UserModel
        {
            get { return _userModel; }
            set { _userModel = value; }
        }


        private String _myMessage;
        public String MyMessage
        {
            get { return _myMessage; }
            set { _myMessage = value; OnPropertyChanged("MyMessage"); }
        }
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
        public int Port
        {
            get { return UserModel.Port; }
            set { UserModel.Port = value; OnPropertyChanged("Port"); }
        }
        

        public UserViewModel(User UserModel)
        {
            _updateFirstNameCommand = new UpdateFirstName(this);
            _listenCommand = new ListenCommand(this);
            _connectCommand = new ConnectCommand(this);
            _userModel = UserModel;
            _userModel.PropertyChanged += myModel_PropertyChanged;
            DisplayName = UserModel.DisplayName;
            MyMessage = "This is Empty";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String PropertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }


        private void myModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName) 
            {
                case "Message":
                    MyMessage = UserModel.Message;
                    break;
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
                default:
                    break;
            }

        }

        private void ShowInvitationMessageBox(string name = "Blabla")
        {
            // Configure the message box to be displayed
            string messageBoxText = $"Do you want to chat with {name}?"; //Name of the person should be added as well later.
            string caption = "Permission";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            // Display message box and save the result
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            // Process message box results
            switch (result)
            {
                case MessageBoxResult.Yes:
                    UserModel.AcceptRequest = true;
                    // should navigate to a new ViewModel
                    
                    break;
                case MessageBoxResult.No:
                    UserModel.AcceptRequest = false;
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
            MessageBox.Show(messageBoxText, caption, button, icon);
        }
        
        private void ShowResponseToRequestMessageBox()
        {
            // Configure the message box to be displayed
            string messageBoxText = "Your request is accepted.";
            if (!UserModel.ResponseToRequest)
            {
                messageBoxText = "Your request is denied.";
            }
            string caption = "Information";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(messageBoxText, caption, button, icon);
        }


        #region Commands
        private UpdateFirstName _updateFirstNameCommand;
        public ICommand UpdateFirstNameCommand => _updateFirstNameCommand;

        private ListenCommand _listenCommand;
        public ICommand ListenCommand => _listenCommand;

        private ConnectCommand _connectCommand;
        public ICommand ConnectCommand => _connectCommand;

        #endregion

    }
}
