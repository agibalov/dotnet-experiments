using System.Windows;

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
    }
}
