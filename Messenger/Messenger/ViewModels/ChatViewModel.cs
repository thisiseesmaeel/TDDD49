using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Messenger.Models;

namespace Messenger.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableCollection<Message> Chatlog { set; get; }

        private User UserModel { set; get; }

        public ChatViewModel(User UM)
        {
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
                    //Chatlog.Add(UserModel.Message);
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

    }
}
