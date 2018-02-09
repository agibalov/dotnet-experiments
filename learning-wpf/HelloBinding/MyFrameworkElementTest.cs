using System.Windows;
using System.Windows.Data;
using NUnit.Framework;

namespace HelloBinding
{
    public class MyFrameworkElementTest
    {
        [Test]
        [RequiresSTA]
        public void Dummy()
        {
            var someViewModel = new SomeViewModel();
            var binding = new Binding("Text") { Source = someViewModel };

            var myFrameworkElement = new MyFrameworkElement();
            myFrameworkElement.SetBinding(MyFrameworkElement.TheTextProperty, binding);

            Assert.AreEqual(null, myFrameworkElement.TheText);

            someViewModel.Text = "Another text";
            Assert.AreEqual("Another text", myFrameworkElement.TheText);
        }
    }

    public class MyFrameworkElement : FrameworkElement
    {
        public static readonly DependencyProperty TheTextProperty =
            DependencyProperty.Register("TheText", typeof(string), typeof(MyFrameworkElement), new PropertyMetadata(default(string)));

        public string TheText
        {
            get { return (string)GetValue(TheTextProperty); }
            set { SetValue(TheTextProperty, value); }
        }
    }

    public class SomeViewModel : DependencyObject
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SomeViewModel), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
