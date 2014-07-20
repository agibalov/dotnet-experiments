using System;
using System.Windows;
using System.Windows.Input;

namespace HelloCommand
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AppViewModel
            {
                ShowMessageCommand = new ShowMessageCommand()
            };
        }
    }

    public class AppViewModel
    {
        public ShowMessageCommand ShowMessageCommand { get; set; }
    }

    public class ShowMessageCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MessageBox.Show("I am command");
        }

        public event EventHandler CanExecuteChanged;
    }
}
