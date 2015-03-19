using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using HelloMarkupExtension.Annotations;

namespace HelloMarkupExtension
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new ViewModel();
            DataContext = vm;

            new Thread(() =>
            {
                while (true)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        vm.CurrentTime = DateTime.Now.ToString();
                    });
                    
                    Thread.Sleep(500);
                }
            }){IsBackground = true}.Start();
        }
    }

    [MarkupExtensionReturnType(typeof(string))]
    public class SumExtension : MarkupExtension
    {
        public int A { get; set; }
        public int B { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return string.Format("{0} and {1} is {2}", A, B, A + B);
        }
    }

    [MarkupExtensionReturnType(typeof(Button))]
    public class MakeButtonExtension : MarkupExtension
    {
        public BindingBase Binding { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var b = new Button();
            b.SetBinding(ContentControl.ContentProperty, Binding);
            return b;
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        private string _currentTime;

        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (value == _currentTime) return;
                _currentTime = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
