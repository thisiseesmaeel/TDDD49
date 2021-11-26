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
            startViewModel.UserIntendsToViewHistoryEvent += new StartViewModel.SwitchToHistoryHandler(GoToHistoryEventHandler);
            SelectedViewModel = startViewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private BaseViewModel _selectedViewModel;


        // To send a signal to parent window when BUZZ is accured.
        public delegate void ShakeMyParentWindowHandler();
        public event ShakeMyParentWindowHandler ShakeMyParentWindowEvent;
        //

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
            chatViewModel.UserIntendsToGoBackEvent += new SwitchToStartHandler(GoToStartEventHandler);
            chatViewModel.ShakeMyParentWindowEvent += new ChatViewModel.ShakeMyParentWindowHandler(RaiseShakeMyParentWindowEvent);
            SelectedViewModel = chatViewModel;
        }

        private void GoToStartEventHandler()
        {
            var startViewModel = new StartViewModel();
            startViewModel.UserIntendsToChatEvent += new StartViewModel.SwitchToChatHandler(GoToChatEventHandler);
            startViewModel.UserIntendsToViewHistoryEvent += new StartViewModel.SwitchToHistoryHandler(GoToHistoryEventHandler);
            SelectedViewModel = startViewModel;
        }

        private void GoToHistoryEventHandler(ChatHistory chatHistoryObj)
        {
            var chatHistoryViewModel = new ChatHistoryViewModel(chatHistoryObj);
            chatHistoryViewModel.UserIntendsToGoBackEvent += new SwitchToStartHandler(GoToStartEventHandler);
            SelectedViewModel = chatHistoryViewModel;
        }

        private void RaiseShakeMyParentWindowEvent()
        {
            ShakeMyParentWindowEvent.Invoke();
        }
    }
}
