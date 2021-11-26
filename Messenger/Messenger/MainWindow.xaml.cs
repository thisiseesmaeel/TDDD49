using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Messenger.ViewModels;


namespace Messenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel Context = new MainViewModel();
            DataContext = Context;
            Closing += Context.OnWindowClosing;
            Context.ShakeMyParentWindowEvent += new MainViewModel.ShakeMyParentWindowHandler(ShakeWindow);
        }

        public void ShakeWindow()
        {
            bool condition = true;
            App.Current.Dispatcher.Invoke(() =>
            {
                for(int i = 0; i < 10; i++)
                {
                    if(condition)
                    {
                        this.Left += 10;
                        condition = false;
                    }
                    else
                    {
                        this.Left -= 10;
                        condition = true;
                    }
                    Thread.Sleep(40);
                }


            });
        }
    }
}
