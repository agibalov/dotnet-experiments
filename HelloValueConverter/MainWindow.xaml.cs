using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using HelloValueConverter.Annotations;

namespace HelloValueConverter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AppViewModel
            {
                ShouldBeVisible = true
            };
        }

        private void SetVisibilityToTrueButton_OnClick(object sender, RoutedEventArgs e)
        {
            ((AppViewModel)DataContext).ShouldBeVisible = true;
        }

        private void SetVisibilityToFalseButton_OnClick(object sender, RoutedEventArgs e)
        {
            ((AppViewModel)DataContext).ShouldBeVisible = false;
        }
    }

    public class AppViewModel : INotifyPropertyChanged
    {
        private bool _shouldBeVisible;
        public bool ShouldBeVisible
        {
            get { return _shouldBeVisible; }
            set
            {
                _shouldBeVisible = value;
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
