using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Messenger.Models
{
    public class User : INotifyPropertyChanged
    {
        public User()
        {
            _firstName = "Hadi";
            OnPropertyChanged("Firstname");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string PropertyName ="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        private String _firstName;        

        public String Firstname
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged("Firstname"); }
        }

    }
}
