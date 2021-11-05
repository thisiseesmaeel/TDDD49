using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Messenger.Models;
using System.Windows.Input;
using Messenger.Commands;

namespace Messenger.ViewModels
{
    public class UserViewModel: INotifyPropertyChanged
    {
        
        public UserViewModel()
        {
            _updateFirstNameCommand = new UpdateFirstName(this);
            //_user = new User();
            test = "BLABLABLA";
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String PropertyName)
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }


        private String test;

        public String Test
        {
            get { return test; }
            set { test = value; OnPropertyChanged("Test"); }
        }

        private User _user;

        public User User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("User"); }
        }

        private UpdateFirstName _updateFirstNameCommand;
        public ICommand UpdateFirstNameCommand => _updateFirstNameCommand;

        /*  public UpdateFirstName UpdateFirstNameCommand
          {
              get { return _updateFirstNameCommand; }
              set { _updateFirstNameCommand = value; }
          }   */


        public void TestMethod()
        {
            Test = "New Firstname";
        }




    }
}
