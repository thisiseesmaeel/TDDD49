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
            var startViewModel = new StartViewModel();
            startViewModel.UserIntendsToChatEvent += new StartViewModel.SwitchToChatHandler(GoToChatEventHandler);
            SelectedViewModel = startViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
        }
        
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            BaseViewModel.UserModel.TearDownConnection();
        }


        private void OnPropertyChanged(String PropertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        private void GoToChatEventHandler()
        {
            var chatViewModel = new ChatViewModel();
            chatViewModel.UserIntendsToGoBackEvent += new ChatViewModel.SwitchToMainHandler(GoToMainEventHandler);
            SelectedViewModel = chatViewModel;
        }

        private void GoToMainEventHandler()
        {
            var startViewModel = new StartViewModel();
            startViewModel.UserIntendsToChatEvent += new StartViewModel.SwitchToChatHandler(GoToChatEventHandler);
            SelectedViewModel = startViewModel;
        }
    }
}
