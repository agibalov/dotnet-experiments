using System.Windows.Controls;

namespace HelloArchitecture
{
    public abstract class BasePage : Page
    {
        public object NavParameter { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (DataContext is INavAware)
            {
                ((INavAware)DataContext).OnNavigatedTo(NavParameter);
            }
        }
    }
}
