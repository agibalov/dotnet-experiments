using System.Windows;

namespace HelloWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new AppViewModel
            {
                Name = "loki2302"
            };
        }
    }
}
