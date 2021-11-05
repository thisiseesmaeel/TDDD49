using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Messenger.Models;
using System.Windows.Input;
using Messenger.ViewModels.Commands;

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
        public UserViewModel(User UserModel)
        {
            _updateFirstNameCommand = new UpdateFirstName(this);
            _listenCommand = new ListenCommand(this);
            _connectCommand = new ConnectCommand(this);
            _userModel = UserModel;
            _userModel.PropertyChanged += myModel_PropertyChanged;
            MyMessage = "This is Empty";
        }


        private String _myMessage;

        public String MyMessage
        {
            get { return _myMessage; }
            set { _myMessage = value; OnPropertyChanged("MyMessage"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String PropertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }


        private void myModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Message")
            {
                MyMessage = _userModel.Message;
                OnPropertyChanged("MyMessage");
            }
        }

        //private String _message;

        //public String Message
        //{
        //    get { return _message; }
        //    set { _message = value; OnPropertyChanged("Message"); }
        //}
        //ctr + c + k + c


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
