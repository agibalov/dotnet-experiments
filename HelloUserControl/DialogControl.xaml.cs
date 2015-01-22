using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelloUserControl
{
    public partial class DialogControl : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(DialogControl),
            new PropertyMetadata(""));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty BodyProperty = DependencyProperty.Register(
            "Body", 
            typeof(object), 
            typeof(DialogControl), 
            new UIPropertyMetadata(null));
        public object Body
        {
            get { return GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(
            "Footer", 
            typeof(object), 
            typeof(DialogControl), 
            new UIPropertyMetadata(null));
        public object Footer
        {
            get { return GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        public DialogControl()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
