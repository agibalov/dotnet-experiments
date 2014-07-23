using System.Windows;

namespace HelloValueConverter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AppViewModel
            {
                ShouldBeVisible = true,
                Sex = Sex.Male,
                YesNo = true
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
}
