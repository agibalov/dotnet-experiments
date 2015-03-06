using System;
using System.Windows;
using System.Windows.Markup;

namespace HelloMarkupExtension
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
}
