using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Messenger.Models;
using Messenger.ViewModels.Commands;

namespace Messenger.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<Message> Chatlog { set; get; }

        public User UserModel { set; get; }

        public ChatViewModel(User UM)
        {
            _chatCommand = new ChatCommand(this);
            UserModel = UM;
            UserModel.PropertyChanged += myModel_PropertyChanged;
            Chatlog = new ObservableCollection<Message>();
            Chatlog.CollectionChanged += Chatlog_CollectionChanged;
        }

        private void myModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Message":
                    App.Current.Dispatcher.Invoke(() => { Chatlog.Add(UserModel.Message); });
                    //Dispatcher.CurrentDispatcher.Invoke(() => { Chatlog.Add(UserModel.Message); });

                    break;
                default:
                    break;
            }

        }

        static void Chatlog_CollectionChanged(object aSender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Console.WriteLine("Add");
                    break;

                case NotifyCollectionChangedAction.Move:
                    Console.WriteLine("Move");
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

        private ChatCommand _chatCommand;
        public ICommand ChatCommand => _chatCommand;
    }
}
