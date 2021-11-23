using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.ViewModels.Commands
{
    public class BackToStartCommand : ICommand
    {
        BaseViewModel _baseViewModel;
        public event EventHandler CanExecuteChanged;

        public BackToStartCommand(BaseViewModel baseViewModel)
        {
            _baseViewModel = baseViewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _baseViewModel.RaiseUserIntendsToGoBackEvent();
                
        }
    }
}
