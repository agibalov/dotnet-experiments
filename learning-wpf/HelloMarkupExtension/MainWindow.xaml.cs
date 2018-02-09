using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using HelloMarkupExtension.Annotations;
using Ninject;

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

            DoShowButton.Click += (sender, args) => vm.ShouldDisplay = true;
            DoHideButton.Click += (sender, args) => vm.ShouldDisplay = false;
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

        private bool _shouldDisplay;
        public bool ShouldDisplay
        {
            get { return _shouldDisplay; }
            set
            {
                if (value == _shouldDisplay) return;
                _shouldDisplay = value;
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

    public class InjectExtension : MarkupExtension
    {
        public object Target { get; set; }

        private readonly IKernel _kernel;

        public InjectExtension()
        {
            _kernel = NinjectServiceLocator.Kernel;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            _kernel.Inject(Target);
            return Target;
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        [Inject]
        private BoolToVisibilityConverterService ConverterService { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterService.MakeVisibilityFromBool((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
