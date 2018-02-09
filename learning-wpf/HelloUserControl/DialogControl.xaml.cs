using System.Windows;
using System.Windows.Controls;

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
        }
    }
}
