using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.Models;
using System.ComponentModel;

namespace Messenger.ViewModels
{
    public class MainViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public MainViewModel()
        {
            var UserViewModel = new UserViewModel(new User());
            UserViewModel.UserIntendsToChatEvent += new UserViewModel.SwitchToChatHandler(MyHandler);
            SelectedViewModel = UserViewModel;
            UserModel = UserViewModel.UserModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
        }

        private User UserModel { set; get; }



        private void OnPropertyChanged(String PropertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        private void MyHandler()
        {
            SelectedViewModel = new ChatViewModel(UserModel);
        }
    }
}
