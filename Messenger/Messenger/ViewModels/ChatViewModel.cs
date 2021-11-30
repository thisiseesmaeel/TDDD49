using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Messenger.Models;
using Messenger.ViewModels.Commands;

namespace Messenger.ViewModels
{
    public class ChatViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<Message> Chatlog { set; get; }

        public ChatViewModel()
        {
            _chatCommand = new ChatCommand(this);
            _backToStartCommand = new BackToStartCommand(this);
            _buzzCommand = new BUZZCommand(this);
            UserModel.PropertyChanged += MyViewModelPropertyChanged;
            Chatlog = new ObservableCollection<Message>();
            //Chatlog.CollectionChanged += Chatlog_CollectionChanged;
            

        }
        private string _messageToSend;

        public string MessageToSend
        {
            get { return _messageToSend; }
            set { _messageToSend = value; OnPropertyChanged("MessageToSend"); }
        }


        // To send a signal to parent class (which is MainVieModel) when BUZZ is accured.
        public delegate void ShakeMyParentWindowHandler();
        public event ShakeMyParentWindowHandler ShakeMyParentWindowEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        //
        private void OnPropertyChanged(String PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
        private void MyViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Message":
                    App.Current.Dispatcher.Invoke(() => { Chatlog.Add(UserModel.Message); });
                    break;
                case "Buzz":
                    App.Current.Dispatcher.Invoke(() => { ShakeMyParentWindowEvent.Invoke(); });
                    break;
                default:
                    break;
            }

        }

        // Do we need this? Triggered when an action affect the collection.
        static void Chatlog_CollectionChanged(object aSender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    break;

                case NotifyCollectionChangedAction.Replace:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #region commands
        private ChatCommand _chatCommand;
        public ICommand ChatCommand => _chatCommand;

        private BackToStartCommand _backToStartCommand;
        public ICommand BackToStartCommand => _backToStartCommand;

        private BUZZCommand _buzzCommand;

        public ICommand BUZZCommand => _buzzCommand;

        #endregion
    }
}
