using System.Windows;

namespace DummyWpfApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyButton.Click += (sender, args) =>
            {
                Title = "OMG!!!";
            };
        }
    }
}
