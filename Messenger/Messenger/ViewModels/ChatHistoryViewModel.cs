using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Messenger.Models;
using Messenger.ViewModels.Commands;

namespace Messenger.ViewModels
{
    public class ChatHistoryViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ChatHistoryViewModel(ChatHistory chatHistory)
        {
            _backToStartCommand = new BackToStartCommand(this);
            ChatHistory = chatHistory;
        }
        private ChatHistory _chatHistory;
        public ChatHistory ChatHistory
        { 
            get { return _chatHistory; }
            set { _chatHistory = value; OnPropertyChanged("ChatHistory"); } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String PropertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }



        #region Commands
        private BackToStartCommand _backToStartCommand;
        public ICommand BackToStartCommand => _backToStartCommand;

        #endregion

    }
}
