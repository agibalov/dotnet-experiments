using System.Windows;
using System.Windows.Controls;
using Ninject;

namespace HelloMarkupExtension
{
    public partial class MyUserControl : UserControl
    {
        [Inject]
        private CalculatorService CalculatorService { get; set; }

        public MyUserControl()
        {
            InitializeComponent();
            TheButton.Click += AddNumbersButtonOnClick;
        }

        private void AddNumbersButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var message = string.Format("{0} + {1} = {2}", 2, 3, CalculatorService.AddNumbers(2, 3));
            MessageBox.Show(message);
        }
    }
}
