using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.ViewModels.Commands
{
    public class ShowHistoryCommand : ICommand
    {
        private StartViewModel _startViewModel;
        public ShowHistoryCommand(StartViewModel startViewModel)
        {
            _startViewModel = startViewModel;
        }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("This is the params that being sent: " + parameter.ToString());
            // Implement show history in usermodel to refer to
            _startViewModel.RaiseUserIntendsToViewHistoryEvent(BaseViewModel.UserModel.ChatHistoryDictionary[parameter.ToString()]);
        }
    }
}
