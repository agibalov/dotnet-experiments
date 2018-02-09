using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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

namespace HelloCustomModel
{
    public partial class FancyThingsUserControl : UserControl
    {
        public ObservableCollection<FancyThing> FancyThings
        {
            get { return (ObservableCollection<FancyThing>)GetValue(FancyThingsProperty); }
            set { SetValue(FancyThingsProperty, value); }
        }

        public static readonly DependencyProperty FancyThingsProperty = DependencyProperty.Register(
            "FancyThings",
            typeof(ObservableCollection<FancyThing>),
            typeof(FancyThingsUserControl),
            new FrameworkPropertyMetadata(FancyThingsPropertyChanged));

        public FancyThingsUserControl()
        {
            InitializeComponent();

            FancyThings = new ObservableCollection<FancyThing>();

            DataContext = this;
        }

        private static void FancyThingsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var target = (FancyThingsUserControl)dependencyObject;
            var oldValue = (ObservableCollection<FancyThing>)args.OldValue;
            var newValue = (ObservableCollection<FancyThing>)args.NewValue;

            if (oldValue != null)
            {
                oldValue.CollectionChanged -= target.FancyThingsContentsChanged;
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += target.FancyThingsContentsChanged;
            }

            // TODO: handle the entire newValue
        }

        private void FancyThingsContentsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Debug.WriteLine(args.Action); // TODO: handle the specific action, looks like always one item at a time
        }
    }
}
