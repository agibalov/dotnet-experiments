using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using HelloNoXaml.Annotations;

namespace HelloNoXaml
{
    class Program
    {
        static void Main(string[] args)
        {
            var appViewModel = new AppViewModel();

            var thread = new Thread(() =>
            {
                var appViewModelUpdateTime = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(500)
                };
                appViewModelUpdateTime.Tick += (sender, eventArgs) => appViewModel.CurrentTime = DateTime.Now.ToString();
                appViewModelUpdateTime.Start();

                var appWindow = new Window
                {
                    Title = "Hello No XAML"
                };
                appWindow.Closed += (sender, eventArgs) =>
                {
                    appViewModelUpdateTime.Stop();
                    ((Window) sender).Dispatcher.InvokeShutdown();
                };

                appWindow.DataContext = appViewModel;

                var textBlock = new TextBlock
                {
                    FontSize = 20
                };
                var currentTimeBinding = new Binding("CurrentTime");
                BindingOperations.SetBinding(textBlock, TextBlock.TextProperty, currentTimeBinding);

                appWindow.Content = textBlock;

                appWindow.Show();

                Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }

    public class AppViewModel : INotifyPropertyChanged
    {
        private string _currentTime;
        public string CurrentTime
        {
            get
            {
                return _currentTime;
            }

            set
            {
                _currentTime = value; 
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
