using System.Windows;
using System.Windows.Controls;

namespace HelloUserControl
{
    public partial class UserNameControl : UserControl
    {
        public UserNameControl()
        {
            InitializeComponent();
            
            LayoutRoot.DataContext = this;
        }

        public string UserName
        {
            get
            {
                return (string) GetValue(UserNameProperty);
            }

            set
            {
                SetValue(UserNameProperty, value);
            }
        }

        private static readonly DependencyProperty UserNameProperty = DependencyProperty.Register(
            "UserName",
            typeof (string), 
            typeof (UserNameControl), 
            new PropertyMetadata(""));
    }
}
