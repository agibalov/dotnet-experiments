using System;
using System.Windows;
using System.Windows.Input;

namespace HelloUserControl
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new AppViewModel
            {
                UserName1 = "loki2302",
                UserName2 = "Andrey"
            };
        }
    }

    public class AppViewModel
    {
        public string UserName1 { get; set; }
        public string UserName2 { get; set; }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AppViewModel()
        {
            OkCommand = new ShowMessageBoxCommand("OK clicked!");
            CancelCommand = new ShowMessageBoxCommand("Cancel clicked");
        }
    }

    public class ShowMessageBoxCommand : ICommand
    {
        private readonly string _message;

        public ShowMessageBoxCommand(string message)
        {
            _message = message;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MessageBox.Show(_message);
        }

        public event EventHandler CanExecuteChanged;
    }
}
